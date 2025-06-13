using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : CharacterCommonBehavior
{
    public override float moveSpeed { get; set; } = 5f;

    [Header("Spell Settings")]
    public GameObject spellPrefab; // Prefab chung cho cả đòn đánh thường và sạc
    public Transform spellSpawnPoint;

    [Header("Basic Spell Properties")]
    public float timeBtwBasicSpell = 0.5f; // Thời gian giữa các lần bắn thường
    public float basicSpellForce = 10f;
    public Vector3 basicSpellScale = new Vector3(1f, 1f, 1f); // Kích thước của đạn thường

    [Header("Charged Spell Properties")]
    public float chargeTimeNeeded = 1f; // Thời gian cần giữ để cast chiêu mạnh
    public float chargedSpellForce = 20f; // Lực của chiêu mạnh
    public Vector3 chargedSpellScale = new Vector3(2f, 2f, 1f); // Kích thước của đạn sạc (lớn hơn)
    public string chargedCastTrigger = "ChargedCast"; // Tên Trigger cho animation chiêu mạnh

    private float _timeBtwBasicSpell = 0f;
    private float _chargeTimer = 0f; // Bộ đếm thời gian giữ chuột
    private bool _isCharging = false; // Cờ hiệu đang giữ chuột

    protected void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        _timeBtwBasicSpell -= Time.deltaTime;

        // Xử lý giữ chuột để sạc chiêu
        if (Input.GetMouseButton(0)) // Nếu chuột trái đang được giữ
        {
            if (!_isCharging) // Bắt đầu sạc nếu vừa nhấn
            {
                _isCharging = true;
                _chargeTimer = 0f; // Reset bộ đếm
                // Có thể thêm animation sạc ở đây nếu muốn
                // animator.SetBool("IsCharging", true); // Nếu có parameter IsCharging trong Animator
            }
            _chargeTimer += Time.deltaTime; // Tăng thời gian sạc
        }

        // Xử lý nhả chuột hoặc đã sạc đủ
        if (Input.GetMouseButtonUp(0)) // Nếu nhả chuột trái
        {
            if (_isCharging)
            {
                _isCharging = false;
                // animator.SetBool("IsCharging", false); // Tắt animation sạc

                if (_chargeTimer >= chargeTimeNeeded)
                {
                    // Đã sạc đủ, thực hiện chiêu mạnh
                    AttackCharged();
                }
                else
                {
                    // Nhả ra trước khi sạc đủ, thực hiện tấn công cơ bản (nếu chưa bắn thường trong lúc giữ)
                    if (_timeBtwBasicSpell <= 0) // Chỉ bắn nếu cooldown cho đạn thường đã hết
                    {
                        AttackBasic();
                    }
                }
            }
        }
        else if (!_isCharging && Input.GetMouseButton(0) && _timeBtwBasicSpell <= 0) // Nhấn giữ nhưng không trong trạng thái sạc (dùng để bắn liên tục khi nhấn giữ)
        {
            AttackBasic();
        }
    }

    // Hàm tấn công cơ bản (bắn liên tục)
    public void AttackBasic()
    {
        _timeBtwBasicSpell = timeBtwBasicSpell;



        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        Vector2 direction = (mouseWorldPos - spellSpawnPoint.position).normalized;

        GameObject newSpell = Instantiate(spellPrefab, spellSpawnPoint.position, Quaternion.identity);
        newSpell.transform.localScale = basicSpellScale; // Thiết lập kích thước cho đạn thường

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        newSpell.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Rigidbody2D spellRb = newSpell.GetComponent<Rigidbody2D>();
        if (spellRb != null)
        {
            spellRb.AddForce(direction * basicSpellForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogWarning("Spell Prefab does not have a Rigidbody2D component!");
        }
    }

    // Hàm tấn công mạnh (chiêu sạc)
    public void AttackCharged()
    {


        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        Vector2 direction = (mouseWorldPos - spellSpawnPoint.position).normalized;

        GameObject newChargedSpell = Instantiate(spellPrefab, spellSpawnPoint.position, Quaternion.identity); // Vẫn dùng spellPrefab
        newChargedSpell.transform.localScale = chargedSpellScale; // Thiết lập kích thước cho đạn sạc

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        newChargedSpell.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Rigidbody2D chargedSpellRb = newChargedSpell.GetComponent<Rigidbody2D>();
        if (chargedSpellRb != null)
        {
            chargedSpellRb.AddForce(direction * chargedSpellForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogWarning("Spell Prefab does not have a Rigidbody2D component!");
        }
    }

    // Bạn có thể bỏ qua hàm Attack() cũ hoặc thay đổi nó nếu muốn
    // Hiện tại, Attack() cũ sẽ không được gọi trực tiếp nữa với logic mới này
    public override void Attack()
    {
        // Hàm này không còn cần thiết với logic mới.
        // Logic tấn công cơ bản và sạc được xử lý trong AttackBasic() và AttackCharged().
    }


    internal void TakeDamage(int damage)
    {
        throw new System.NotImplementedException();
    }
}
