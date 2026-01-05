using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleController : MonoBehaviour {
    public static BattleController instance {get; private set;}
    
    public int startingMana = 4, maxMana = 12;
    public int playerMana, enemyMana;
    private int currentPlayerMaxMana, currentEnemyMaxMana;
    private int startingCardsAmount = 5;

    public enum TurnOrder {playerActive, playerCardAttacks, enemyActive, enemyCardAttacks}
    public TurnOrder currentPhase;
    public int cardsToDrawPerTurn = 2;
    public Transform discardPoint;
    public int playerHealth, enemyHealth;

    public bool battleEnded;
    [Range(0f, 1f)]
    public float playerFirstChance = .5f;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        currentPlayerMaxMana = startingMana;
        FillPlayerMana();
        currentEnemyMaxMana = startingMana;
        FillEnemyMana();

        DeckController.instance.DrawMultipleCards(startingCardsAmount);
        BattleCardsBattleUI.instance.SetPlayerHealthText(playerHealth);
        BattleCardsBattleUI.instance.SetEnemyHealthText(enemyHealth);

        if (Random.value > playerFirstChance) {
            currentPhase = TurnOrder.playerCardAttacks;
            AdvanceTurn();
        }

    }

    private void Update() {
        if (Keyboard.current.tKey.wasPressedThisFrame) {
            AdvanceTurn();
        }
    }

    public void SpendPlayerMana(int amountToSpend) {
        playerMana -= amountToSpend;
        if (playerMana < 0) {
            playerMana = 0;
        }
        BattleCardsBattleUI.instance.SetPlayerManaText(playerMana);
    }

    public void FillPlayerMana() {
        playerMana = currentPlayerMaxMana;
        BattleCardsBattleUI.instance.SetPlayerManaText(playerMana);
    }

    public void SpendEnemyMana(int amountToSpend) {
        enemyMana -= amountToSpend;
        if (enemyMana < 0) {
            enemyMana = 0;
        }
        BattleCardsBattleUI.instance.SetEnemyManaText(enemyMana);
    }

    public void FillEnemyMana() {
        enemyMana = currentEnemyMaxMana;
        BattleCardsBattleUI.instance.SetEnemyManaText(enemyMana);
    }

    public void AdvanceTurn() {
        if (battleEnded) {
            return;
        }
        currentPhase++;

        if ((int)currentPhase >= System.Enum.GetValues(typeof(TurnOrder)).Length) {
            currentPhase = 0;
        }

        switch (currentPhase) {
            
            case TurnOrder.playerActive:
                BattleCardsBattleUI.instance.ShowEndTurnButton();
                BattleCardsBattleUI.instance.ShowDrawCardButton();

                if (currentPlayerMaxMana < maxMana) {
                    currentPlayerMaxMana++;
                }
                FillPlayerMana();
                DeckController.instance.DrawMultipleCards(cardsToDrawPerTurn);
                break;
            case TurnOrder.playerCardAttacks:
                CardPointsController.instance.PlayerAttack();
                break;
            case TurnOrder.enemyActive:
                if (currentEnemyMaxMana < maxMana) {
                    currentEnemyMaxMana++;
                }
                FillEnemyMana();

                EnemyController.instance.StartAction();
                break;
            case TurnOrder.enemyCardAttacks:
                CardPointsController.instance.EnemyAttack();
                break;
        }
    }

    public void EndPlayerTurn() {
        BattleCardsBattleUI.instance.HideEndTurnButton();
        BattleCardsBattleUI.instance.HideDrawCardButton();
        AdvanceTurn();
    }

    public void DamagePlayer(int damageAmount) {
        if (playerHealth > 0 || !battleEnded) {
            playerHealth -= damageAmount;
            if (playerHealth <= 0) {
                playerHealth = 0;

                //end battle
                EndBattle();
            }
            BattleCardsBattleUI.instance.SetPlayerHealthText(playerHealth);

            DamageIndicatorUI damageClone = Instantiate(BattleCardsBattleUI.instance.playerDamage, BattleCardsBattleUI.instance.playerDamage.transform.parent);
            damageClone.damageText.text = damageAmount.ToString();
            damageClone.gameObject.SetActive(true);
        }
    }
    public void DamageEnemy(int damageAmount) {
        if (enemyHealth > 0 || !battleEnded) {
            enemyHealth -= damageAmount;
            if (enemyHealth <= 0) {
                enemyHealth = 0;

                //end battle
                EndBattle();
            }
            BattleCardsBattleUI.instance.SetEnemyHealthText(enemyHealth);

            DamageIndicatorUI damageClone = Instantiate(BattleCardsBattleUI.instance.enemyDamage, BattleCardsBattleUI.instance.enemyDamage.transform.parent);
            damageClone.damageText.text = damageAmount.ToString();
            damageClone.gameObject.SetActive(true);
        }
    }

    private void EndBattle() {
        battleEnded = true;
        HandController.instance.EmptyHand();

        if (enemyHealth <= 0) {
            BattleCardsBattleUI.instance.SetBattleResults("YOU WON");
            // get rid of every card on enemy side
            foreach (CardPlacePoint point in CardPointsController.instance.enemyCardPoints) {
                if (point.activeCard != null) {
                    point.activeCard.MoveToPoint(discardPoint.position, point.activeCard.transform.rotation);
                }
            }

        } else {
            BattleCardsBattleUI.instance.SetBattleResults("YOU LOST");
            // get rid of every card on player's side
            foreach (CardPlacePoint point in CardPointsController.instance.playerCardPoints) {
                if (point.activeCard != null) {
                    point.activeCard.MoveToPoint(discardPoint.position, point.activeCard.transform.rotation);
                }
            }
        }

        StartCoroutine(ShowResultsCo());
    }

    IEnumerator ShowResultsCo() {
        yield return new WaitForSeconds(1f);
        BattleCardsBattleUI.instance.battleEndScreen.SetActive(true);
    }

}
