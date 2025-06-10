using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour
{
    [Header("Character Data")]
    public CharacterData[] allCharacters;

    [Header("UI References")]
    public Transform characterGrid;
    public GameObject characterCardPrefab;
    public Button startButton;

    [Header("Character Info Panel")]
    public Image characterPortrait;
    public Text characterNameText;
    public Text characterDescriptionText;
    public Text healthText;
    public Text speedText;
    public Text damageText;
    public Text weaponText;

    private CharacterCard selectedCard;
    private CharacterData selectedCharacter;

    void Start()
    {
        SetupCharacterGrid();
        startButton.onClick.AddListener(StartGame);

        // Select first unlocked character by default
        SelectFirstUnlockedCharacter();
    }

    void SetupCharacterGrid()
    {
        // Clear existing cards
        foreach (Transform child in characterGrid)
        {
            Destroy(child.gameObject);
        }

        // Create character cards
        foreach (CharacterData character in allCharacters)
        {
            GameObject cardObj = Instantiate(characterCardPrefab, characterGrid);
            CharacterCard card = cardObj.GetComponent<CharacterCard>();
            card.SetupCard(character, this);
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
        selectedCharacter = card.GetCharacterData();
        card.SetSelected(true);

        // Update character info panel
        UpdateCharacterInfo();

        // Enable start button
        startButton.interactable = true;
    }

    void UpdateCharacterInfo()
    {
        if (selectedCharacter == null) return;

        characterPortrait.sprite = selectedCharacter.characterPortrait;
        characterNameText.text = selectedCharacter.characterName;
        characterDescriptionText.text = selectedCharacter.description;
        healthText.text = "Health: " + selectedCharacter.health;
        speedText.text = "Speed: " + selectedCharacter.speed;
        damageText.text = "Damage: " + selectedCharacter.damage;
        weaponText.text = "Starting Weapon: " + selectedCharacter.startingWeapon;
    }

    void SelectFirstUnlockedCharacter()
    {
        foreach (Transform child in characterGrid)
        {
            CharacterCard card = child.GetComponent<CharacterCard>();
            if (card.GetCharacterData().isUnlocked)
            {
                SelectCharacter(card);
                break;
            }
        }
    }

    void StartGame()
    {
        if (selectedCharacter != null)
        {
            // L?u character ?ã ch?n
            PlayerPrefs.SetString("SelectedCharacter", selectedCharacter.name);

            // Chuy?n scene
            SceneManager.LoadScene("GameScene");
        }
    }
}