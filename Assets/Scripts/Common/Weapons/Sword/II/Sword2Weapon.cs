using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword2Weapon : MonoBehaviour
{
    float timeToAttack = 4f;
    float timer;
    public GameObject right_Weapon;
    public GameObject left_Weapon;
    private CharacterCommonBehavior characterCommonBehavior;
    private GameObject player;
    private Vector3 sizeCharacter;
    private const string PLAYER_TAG = "Player";
    private float delayAttack = 0.3f;
    private void Start()
    {
        characterCommonBehavior = FindObjectOfType<CharacterCommonBehavior>();
        player = GameObject.FindWithTag(PLAYER_TAG);
        sizeCharacter = player.GetComponent<Collider2D>().bounds.size;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            AttackRight();
            AttackLeft();
        }
    }

    void AttackRight()
    {
        timer = timeToAttack;
        Vector3 offset = new Vector3(sizeCharacter.x + 4f, 0, 0);
        right_Weapon.transform.position = player.transform.position + offset;
        right_Weapon.SetActive(true);
        delayAttack = Time.deltaTime + delayAttack;
    }

    void AttackLeft()
    {
        timer = timeToAttack;
        Vector3 offset = new Vector3(-(sizeCharacter.x + 4f), 0, 0);
        left_Weapon.transform.position = player.transform.position + offset;
        left_Weapon.SetActive(true);
    }
}
