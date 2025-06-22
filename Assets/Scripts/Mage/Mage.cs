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
    public string isChargingBoolParam = "IsCharging"; // Tên parameter bool cho animation sạc

    private float _timeBtwBasicSpell = 0f;
    private float _chargeTimer = 0f; // Bộ đếm thời gian giữ chuột
    private bool _isCharging = false; // Cờ hiệu đang giữ chuột
    private bool _animationStarted = false; // Cờ hiệu kiểm soát animation sạc đã bắt đầu chưa

    private Animator _mageAnimator;

    protected override void Start()
    {
        base.Start();
        _mageAnimator = GetComponent<Animator>();
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
                _animationStarted = false; // Reset cờ hiệu animation
            }
            _chargeTimer += Time.deltaTime; // Tăng thời gian sạc

            // Kiểm tra nếu thời gian giữ chuột đã đủ để bắt đầu animation sạc
            // VÀ animation chưa bắt đầu
            if (_chargeTimer >= chargeTimeNeeded && !_animationStarted)
            {
                if (_mageAnimator != null)
                {
                    _mageAnimator.SetBool(isChargingBoolParam, true); // Bật animation sạc
                }
                _animationStarted = true; // Đặt cờ hiệu là animation đã bắt đầu
            }
        }

        // Xử lý nhả chuột hoặc đã sạc đủ
        if (Input.GetMouseButtonUp(0)) // Nếu nhả chuột trái
        {
            if (_isCharging)
            {
                _isCharging = false;
                _animationStarted = false; // Reset cờ hiệu animation khi nhả chuột

                // Tắt animation sạc (nếu nó đang chạy)
                if (_mageAnimator != null)
                {
                    _mageAnimator.SetBool(isChargingBoolParam, false); // Tắt animation sạc
                }

                if (_chargeTimer >= chargeTimeNeeded)
                {
                    // Đã sạc đủ, thực hiện chiêu mạnh
                    AttackCharged();
                }
                else
                {
                    // Nhả ra trước khi sạc đủ, thực hiện tấn công cơ bản
                    if (_timeBtwBasicSpell <= 0) // Chỉ bắn nếu cooldown cho đạn thường đã hết
                    {
                        AttackBasic();
                    }
                }
            }
        }
        // Logic bắn liên tục khi nhấn giữ và không sạc (giữ nguyên)
        else if (!_isCharging && Input.GetMouseButton(0) && _timeBtwBasicSpell <= 0)
        {
            AttackBasic();
        }
    }

    // Các hàm AttackBasic, AttackCharged, Attack giữ nguyên
    public void AttackBasic()
    {
        _timeBtwBasicSpell = timeBtwBasicSpell;


        // ... (phần còn lại của hàm AttackBasic) ...
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

    public void AttackCharged()
    {


        // ... (phần còn lại của hàm AttackCharged) ...
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        Vector2 direction = (mouseWorldPos - spellSpawnPoint.position).normalized;

        GameObject newChargedSpell = Instantiate(spellPrefab, spellSpawnPoint.position, Quaternion.identity);
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

    public override void Attack()
    {
        // Hàm này không còn cần thiết với logic mới.
    }
}
