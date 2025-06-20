using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeapon : MonoBehaviour
{

    float timeToAttack = 4f;
    float timer;
    public GameObject right_Weapon;
    public GameObject left_Weapon;
    private CharacterCommonBehavior characterCommonBehavior;
    //private Rigidbody2D rigidbody2;
    private GameObject player;
    private Vector3 sizeCharacter;
    private void Start()
    {
        characterCommonBehavior = FindObjectOfType<CharacterCommonBehavior>();
        player = GameObject.FindWithTag("Player");
        sizeCharacter = player.GetComponent<Collider2D>().bounds.size;
    }

    // Update is called once per frame
    void Update()
    {
      //  rigidbody2 = characterCommonBehavior.GetRigidbody2D();
        timer -= Time.deltaTime;
        if (timer <= 0)
        {

            if (characterCommonBehavior.GetOnMovingCharacterHorizontal() > 0f)
            {
                AttackRight();
            }
            if (characterCommonBehavior.GetOnMovingCharacterHorizontal() <= 0f)
            {
                AttackLeft();
            }
        }

    }

    void AttackRight()
    {
        timer = timeToAttack;
        Vector3 offset = new Vector3(sizeCharacter.x + 2f, 0, 0);
        right_Weapon.transform.position = player.transform.position + offset;
        right_Weapon.SetActive(true);
    }

    void AttackLeft()
    {
        timer = timeToAttack;
        Vector3 offset = new Vector3(-(sizeCharacter.x + 2f), 0, 0);
        left_Weapon.transform.position =player.transform.position + offset ;
        left_Weapon.SetActive(true);
    }
}