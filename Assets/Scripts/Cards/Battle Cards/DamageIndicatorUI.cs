using TMPro;
using UnityEngine;

public class DamageIndicatorUI : MonoBehaviour {
    public TMP_Text damageText;
    [SerializeField] private float moveSpeed = 100f;
    [SerializeField] private float lifetime = 3f;
    private RectTransform myRect;

    private void Start() {
        Destroy(gameObject, lifetime);
        myRect = GetComponent<RectTransform>();
    }

    private void Update() {
        myRect.anchoredPosition += new Vector2(0f, -moveSpeed * Time.deltaTime);
    }

}