using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI References")]
    public Image characterImage;
    public Image cardBackground;
    public Image selectionBorder;
    public GameObject lockOverlay;
    public GameObject lockIcon;
    public TextMeshProUGUI characterNameText; // Hoặc Text nếu dùng Legacy Text

    [Header("Card Data")]
    public CharacterData characterData;
    public bool isSelected = false;
    public bool isLocked = false;

    [Header("Visual Settings")]
    public Color normalColor = Color.white;
    public Color hoverColor = new Color(0.9f, 0.95f, 1f, 1f);
    public Color selectedColor = new Color(1f, 1f, 0.6f, 1f);
    public Color lockedColor = Color.gray;

    private Button cardButton;
    private CharacterSelectManager characterSelector;

    void Awake()
    {
        cardButton = GetComponent<Button>();

        // Setup button click event
        if (cardButton != null)
        {
            cardButton.onClick.AddListener(OnCardClicked);
        }
    }

    void Start()
    {
        // Tìm CharacterSelectManager sau khi scene đã load
        if (characterSelector == null)
        {
            characterSelector = FindObjectOfType<CharacterSelectManager>();
        }
    }

    public void SetupCard(CharacterData data)
    {
        if (data == null) return;

        characterData = data;

        // Set basic info
        if (characterNameText != null)
        {
            characterNameText.text = data.characterName;
        }

        if (characterImage != null && data.characterSprite != null)
        {
            characterImage.sprite = data.characterSprite;
        }

        // Set lock status
        isLocked = data.isLocked;

        if (lockOverlay != null)
        {
            lockOverlay.SetActive(isLocked);
        }

        if (lockIcon != null)
        {
            lockIcon.SetActive(isLocked);
        }

        // Set interactable state
        if (cardButton != null)
        {
            cardButton.interactable = !isLocked;
        }

        // Update visual state
        UpdateVisuals();
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;

        if (selectionBorder != null)
        {
            selectionBorder.gameObject.SetActive(selected);
        }

        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (cardBackground == null) return;

        if (isLocked)
        {
            cardBackground.color = lockedColor;
        }
        else if (isSelected)
        {
            cardBackground.color = selectedColor;
        }
        else
        {
            cardBackground.color = normalColor;
        }
    }

    private void OnCardClicked()
    {
        if (!isLocked && characterSelector != null)
        {
            characterSelector.SelectCharacter(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isLocked && !isSelected && cardBackground != null)
        {
            cardBackground.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isLocked && !isSelected && cardBackground != null)
        {
            cardBackground.color = normalColor;
        }
    }
}