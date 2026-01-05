using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Represents a card to be played during card battle.
/// </summary>
public class Card : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler {
    public CardInfo cardInfo;
    public bool isPlayer;
    public int attackPower, currentHealth, manaCost;
    [SerializeField] private TMP_Text attackText, healthText, manaCostText;
    [SerializeField] private TMP_Text nameText, actionDescriptionText, loreText;
    [SerializeField] private Image characterArt, bgArt;

    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float rotateSpeed = 720f;
    [SerializeField] private LayerMask whatIsDesktop, whatIsPlacement;

    public bool inHand;
    public int handPosition;

    private Vector3 targetPoint;
    private Quaternion targetRot;

    private bool isHovered;
    private bool isDragging;

    private HandController theHC;
    public CardPlacePoint assignedPlace;
    public Animator anim;
    private const string ANIMATION_JUMP = "Jump";
    private const string ANIMATION_HURT = "Hurt";
    

    private void Start() {
        if (targetPoint == Vector3.zero) {
            targetPoint = transform.position;
            targetRot = transform.rotation;
        }
        SetupCard();
        theHC = FindAnyObjectByType<HandController>();
    }

    private void Update() {
        transform.position = Vector3.Lerp(transform.position, targetPoint, moveSpeed * Time.deltaTime);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
    }


    public void OnPointerEnter(PointerEventData eventData) {
        if (!inHand || isDragging || isHovered || !isPlayer && BattleController.instance.battleEnded && Time.timeScale == 0f) return;

        isHovered = true;

        MoveToPoint(theHC.cardPositions[handPosition] + new Vector3(0f, 1f, 0.4f),Quaternion.identity);
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (!inHand || isDragging || !isHovered || !isPlayer && BattleController.instance.battleEnded && Time.timeScale == 0f) return;

        isHovered = false;

        ReturnToHand();
    }


    public void OnBeginDrag(PointerEventData eventData) {
        if (!inHand && BattleController.instance.currentPhase != BattleController.TurnOrder.playerActive && !isPlayer && BattleController.instance.battleEnded && Time.timeScale == 0f) return;
        
        isDragging = true;
        isHovered = false;
    }

    public void OnDrag(PointerEventData eventData) {
        if (!isDragging || !inHand && BattleController.instance.currentPhase != BattleController.TurnOrder.playerActive && !isPlayer && BattleController.instance.battleEnded && Time.timeScale == 0f) return;

        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, whatIsDesktop)) {
            MoveToPoint(hit.point + Vector3.up * 2f, Quaternion.identity);
        }

    }

    public void OnEndDrag(PointerEventData eventData) {
        if (BattleController.instance.battleEnded && Time.timeScale == 0f) {
            return;
        }

        isDragging = false;
        // if user stops dragging on a placement point, place card there
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, whatIsPlacement) && BattleController.instance.currentPhase == BattleController.TurnOrder.playerActive) {
            CardPlacePoint selectedPoint = hit.collider.GetComponent<CardPlacePoint>();

            if (selectedPoint.activeCard == null && selectedPoint.isPlayerPoint) {

                if (BattleController.instance.playerMana >= cardInfo.manaCost) {
                    selectedPoint.activeCard = this;
                    assignedPlace = selectedPoint;

                    MoveToPoint(selectedPoint.transform.position, Quaternion.identity);
                    inHand = false;
                    theHC.RemoveCardFromHand(this);
                    BattleController.instance.SpendPlayerMana(cardInfo.manaCost);
                } else {
                    ReturnToHand();
                    BattleCardsBattleUI.instance.ShowManaWarning();
                }
            } else {
                ReturnToHand();
            }

        } else {
            ReturnToHand();
        }
    }

    private void ReturnToHand() {
        MoveToPoint(theHC.cardPositions[handPosition], theHC.minPos.rotation);
    }

    public void MoveToPoint(Vector3 point, Quaternion rot) {
        targetPoint = point;
        targetRot = rot;
    }

    public void SetupCard() {
        attackPower = cardInfo.attackPower;
        currentHealth = cardInfo.currentHealth;
        manaCost = cardInfo.manaCost;

        UpdateCardDisplay();

        nameText.text = cardInfo.name;
        actionDescriptionText.text = cardInfo.actionDescription;
        loreText.text = cardInfo.cardLore;

        characterArt.sprite = cardInfo.characterSprite;
        bgArt.sprite = cardInfo.bgSprite;
    }

    public void DamageCard(int damageAmount) {
        currentHealth -= damageAmount;
        if (currentHealth <= 0) {
            currentHealth = 0;
            assignedPlace.activeCard = null;
            MoveToPoint(BattleController.instance.discardPoint.position, BattleController.instance.discardPoint.rotation);
            anim.SetTrigger(ANIMATION_JUMP);
            Destroy(gameObject, 5f);
        }
        anim.SetTrigger(ANIMATION_HURT);
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay() {
        attackText.text = attackPower.ToString();
        healthText.text = currentHealth.ToString();
        manaCostText.text = manaCost.ToString();
    }
}
