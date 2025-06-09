using UnityEngine;
using UnityEngine.UI;

public class CharacterCard : MonoBehaviour
{
    [Header("UI References")]
    public Image characterImage;
    public Image cardBackground;
    public Image selectionBorder;
    public Image lockOverlay;
    public Text characterNameText;

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;
    public Color lockedColor = Color.gray;

    private CharacterData characterData;
    private CharacterSelectManager selectManager;
    private Button cardButton;

    void Awake()
    {
        cardButton = GetComponent<Button>();
        cardButton.onClick.AddListener(OnCardClicked);
    }

    public void SetupCard(CharacterData data, CharacterSelectManager manager)
    {
        characterData = data;
        selectManager = manager;

        characterImage.sprite = data.characterSprite;
        characterNameText.text = data.characterName;

        UpdateCardVisual();
    }

    void OnCardClicked()
    {
        if (characterData.isUnlocked)
        {
            selectManager.SelectCharacter(this);
        }
    }

    public void SetSelected(bool selected)
    {
        selectionBorder.gameObject.SetActive(selected);
        cardBackground.color = selected ? selectedColor : normalColor;
    }

    void UpdateCardVisual()
    {
        lockOverlay.gameObject.SetActive(!characterData.isUnlocked);
        cardButton.interactable = characterData.isUnlocked;

        if (!characterData.isUnlocked)
        {
            characterImage.color = lockedColor;
        }
    }

    public CharacterData GetCharacterData() => characterData;
}