using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character Selection/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("Basic Info")]
    public string characterName;
    public Sprite characterSprite;
    public Sprite characterPortrait;

    [Header("Description")]
    [TextArea(3, 5)]
    public string description;

    [Header("Stats")]
    public float health = 100f;
    public float speed = 3.5f;
    public int damage = 15;
    public string weaponName = "Basic Weapon";

    [Header("Unlock Status")]
    public bool isLocked = false;

    [Header("Prefab Reference")]
    public GameObject characterPrefab; // Prefab nhân vật để spawn trong game
}