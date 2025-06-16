using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class J_Boss : BossBase
{
    public GameObject target;
    public GameObject projectile;
    public float bulletForce = 2f;
    public float timeBtwAttack = 2f;
    private float _timeBtwAttack = 2f;

    private CharacterCommonBehavior targetCharacter;

    public void Awake()
    {
        targetCharacter = target.GetComponent<CharacterCommonBehavior>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //check if distance to target is less than 10 units
        if (Vector3.Distance(transform.position, target.transform.position) < 10f)
        {
            _timeBtwAttack -= Time.deltaTime;
            if (_timeBtwAttack <= 0f)
            {
                Attack1();
                _timeBtwAttack = timeBtwAttack;
            }
        }
    }

    private void Attack1()
    {
        Debug.Log("Attack");
        _timeBtwAttack = timeBtwAttack;

        Vector2 direction1 = ((Vector2)targetCharacter.transform.position - (Vector2)transform.position).normalized;
        Vector2 direction2 = Quaternion.Euler(0, 0, 10) * direction1; 
        Vector2 direction3 = Quaternion.Euler(0, 0, -10) * direction1; 
        Vector2 direction4 = Quaternion.Euler(0, 0, 20) * direction1; 
        Vector2 direction5 = Quaternion.Euler(0, 0, -20) * direction1;

        for (int i = 0; i < 5; i++)
        {
            Vector2 direction;
            switch (i)
            {
                case 0: direction = direction1; break;
                case 1: direction = direction2; break;
                case 2: direction = direction3; break;
                case 3: direction = direction4; break;
                case 4: direction = direction5; break;
                default: direction = direction1; break;
            }
            GameObject newArrow = Instantiate(projectile, transform.position, Quaternion.identity);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            newArrow.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            Rigidbody2D arrowRb = newArrow.GetComponent<Rigidbody2D>();
            arrowRb.AddForce(direction * bulletForce, ForceMode2D.Impulse);
        }
    }
}
