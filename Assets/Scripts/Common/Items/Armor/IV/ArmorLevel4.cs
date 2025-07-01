using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorLevel4 : MonoBehaviour
{
    private GameObject player;
    private const string PLAYER_TAG = "Player";
    private CharacterCommonBehavior characterCommonBehavior;
    private float newCharacterBaseArmorPlus = 5f;
    private float healRegenationPerSeconds = 1f;
    private float currentHealPoint;
    private bool isMaxHp;
    void Start()
    {
        player = GameObject.FindWithTag(PLAYER_TAG);
        characterCommonBehavior = player.GetComponent<CharacterCommonBehavior>();
        float baseCharaterArmmor = characterCommonBehavior.GetCharacterBaseArmor();
        characterCommonBehavior.SetCharacterBaseArmor(baseCharaterArmmor + newCharacterBaseArmorPlus);
        transform.SetParent(player.transform);

    }
    private void Update()
    {
        currentHealPoint = characterCommonBehavior.hp;

    }
}
