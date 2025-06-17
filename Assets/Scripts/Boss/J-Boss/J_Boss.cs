using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class J_Boss : EnemyBase
{
    public GameObject target;
    public GameObject projectile;
    public float bulletForce = 2f;
    public float timeBtwAttack = 2f;
    private float _timeBtwAttack = 2f;

    public float timeBtwAttack2 = 2f;
    private float _timeBtwAttack2 = 2f;

    public GameObject cirleFlame;

    private CharacterCommonBehavior targetCharacter;

    protected override void Awake()
    {
        base.Awake();
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

        if (Vector3.Distance(transform.position, target.transform.position) < 15f)
        {
            _timeBtwAttack2 -= Time.deltaTime;
            if (_timeBtwAttack2 <= 0f)
            {
                Attack2();
                _timeBtwAttack2 = timeBtwAttack2;
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 direction = (targetCharacter.transform.position - transform.position).normalized;
        rgb2d.velocity = direction * speed;
    }

    private void Attack1()
    {
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

    private void Attack2()
    {
        Debug.Log("Skill2");

        _timeBtwAttack2 = timeBtwAttack2;
        Instantiate(cirleFlame, target.transform.position, Quaternion.identity);
    }
}
