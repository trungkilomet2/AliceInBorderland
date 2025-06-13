using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class CharacterSelectManager : MonoBehaviour
{
    [Header("UI References")]
    public Transform characterGrid;
    public GameObject characterCardPrefab;

    [Header("Character Info Panel")]
    public Image characterPortrait;
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI weaponText;

    [Header("Buttons")]
    public Button startButton;

    [Header("Character Database")]
    public CharacterData[] availableCharacters;

    [Header("Settings")]
    public string gameSceneName = "SampleScene";

    // Private variables
    private List<CharacterCard> characterCards = new List<CharacterCard>();
    private CharacterCard selectedCard;
    private CharacterData selectedCharacterData;

    void Start()
    {
        InitializeCharacterGrid();
        SetupButtons();
    }

    void InitializeCharacterGrid()
    {
        // Clear existing cards
        foreach (Transform child in characterGrid)
        {
            Destroy(child.gameObject);
        }
        characterCards.Clear();

        // Create character cards
        foreach (CharacterData character in availableCharacters)
        {
            GameObject cardObj = Instantiate(characterCardPrefab, characterGrid);
            CharacterCard card = cardObj.GetComponent<CharacterCard>();

            if (card != null)
            {
                card.SetupCard(character);
                characterCards.Add(card);
            }
        }
    }

    void SetupButtons()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
            startButton.interactable = false; 
        }
    }

    void SelectFirstAvailableCharacter()
    {
        foreach (CharacterCard card in characterCards)
        {
            if (!card.isLocked)
            {
                SelectCharacter(card);
                break;
            }
        }
    }

    public void SelectCharacter(CharacterCard card)
    {
        // Deselect previous card
        if (selectedCard != null)
        {
            selectedCard.SetSelected(false);
        }

        // Select new card
        selectedCard = card;
        selectedCharacterData = card.characterData;
        card.SetSelected(true);

        // Update character info panel
        UpdateCharacterInfo();

        // Enable start button
        if (startButton != null)
        {
            startButton.interactable = true;
        }
    }

    void UpdateCharacterInfo()
    {
        if (selectedCharacterData == null) return;

        // Update portrait
        if (characterPortrait != null)
        {
            characterPortrait.sprite = selectedCharacterData.characterPortrait != null ?
                selectedCharacterData.characterPortrait : selectedCharacterData.characterSprite;
        }

        // Update texts
        if (characterNameText != null)
            characterNameText.text = selectedCharacterData.characterName;

        if (descriptionText != null)
            descriptionText.text = selectedCharacterData.description;

        if (healthText != null)
            healthText.text = $"Health: {selectedCharacterData.health}";

        if (speedText != null)
            speedText.text = $"Speed: {selectedCharacterData.speed}";

        if (damageText != null)
            damageText.text = $"Damage: {selectedCharacterData.damage}";

        if (weaponText != null)
            weaponText.text = $"Weapon: {selectedCharacterData.weaponName}";
    }

    public void StartGame()
    {
        if (selectedCharacterData != null)
        {
            PlayerPrefs.SetString("SelectedCharacter", selectedCharacterData.name);
            PlayerPrefs.Save();

            SceneManager.LoadScene(gameSceneName);
        }
    }

    // Public methods for external access
    public CharacterData GetSelectedCharacter()
    {
        return selectedCharacterData;
    }

    public void RefreshCharacterGrid()
    {
        InitializeCharacterGrid();
        SelectFirstAvailableCharacter();
    }
}