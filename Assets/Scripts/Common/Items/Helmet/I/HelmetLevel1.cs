using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetLevel1 : MonoBehaviour
{
    private GameObject player;
    private const string PLAYER_TAG = "Player";
    private CharacterCommonBehavior characterCommonBehavior;
    private float newCharacterBaseArmorPlus = 2.5f;
    void Start()
    {
        player = GameObject.FindWithTag(PLAYER_TAG);
        characterCommonBehavior = player.GetComponent<CharacterCommonBehavior>();
        float baseCharaterArmmor = characterCommonBehavior.GetCharacterBaseArmor();
        characterCommonBehavior.SetCharacterBaseArmor(baseCharaterArmmor + newCharacterBaseArmorPlus);
        transform.SetParent(player.transform);
    }
}
