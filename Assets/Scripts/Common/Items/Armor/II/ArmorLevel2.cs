using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorLevel2 : MonoBehaviour
{
    private GameObject player;
    private const string PLAYER_TAG = "Player";
    private CharacterCommonBehavior characterCommonBehavior;
    private float newCharacterBaseArmor = 15.5f;
    void Start()
    {
        player = GameObject.FindWithTag(PLAYER_TAG);
        characterCommonBehavior = player.GetComponent<CharacterCommonBehavior>();
        characterCommonBehavior.SetCharacterBaseArmor(newCharacterBaseArmor);
        transform.SetParent(player.transform);
    }
}
