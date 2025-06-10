using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Sprite characterSprite;
    public Sprite characterPortrait;
    public string description;
    public bool isUnlocked = true;

    [Header("Starting Stats")]
    public float health = 100f;
    public float speed = 3f;
    public float damage = 10f;
    public string startingWeapon;

    [Header("Special Abilities")]
    public string[] specialAbilities;
}