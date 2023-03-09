using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    Animator animator;

    public float speed = 6f;

    private int comboStep;

    private bool isAttack;

    private bool isGetHurt;

    private float attackGapTimer;

    private float attackGapInterval = 1f;

    private static int MAX_ATTACK_STEP = 3;

    public TextMeshProUGUI attackSetpTip;
    public TextMeshProUGUI attackTimerTip;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Attack();
        UpdateAttackTimer();
        UpdateTip();
    }


    // ???????
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) {
            print("attack enemy");
            collision.GetComponent<EnemyC>()?.OnGetHurt();
        }
    }

    private bool IsDisabledMove() {
        if (isAttack || isGetHurt)
        {
            return true;
        }

        return false;
    }

    private bool IsDisabledAttack()
    {
        if (isGetHurt || isAttack)
        {
            return true;
        }

        return false;
    }

    private void Move()
    {
        if (IsDisabledMove())
        {
            return;
        }

        float inputX = Input.GetAxis("Horizontal");
        float faceDirec = Input.GetAxisRaw("Horizontal");

        if (inputX != 0)
        {
            rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
            animator.SetFloat("running", Mathf.Abs(inputX));
        }

        if (faceDirec != 0)
        {
            transform.localScale = new Vector3(faceDirec, 1, 1);
        }
    }

    private void Attack()
    {
        if (IsDisabledAttack())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            isAttack = true;
            comboStep++;

            if (comboStep > MAX_ATTACK_STEP)
            {
                comboStep = 1;
            }

            attackGapTimer = attackGapInterval;
            animator.SetTrigger("attack");
            animator.SetInteger("comboStep", comboStep);
        }
    }

    private void UpdateAttackTimer()
    {
        if (attackGapTimer != 0)
        {
            attackGapTimer -= Time.deltaTime;

            if (attackGapTimer <= 0)
            {
                attackGapTimer = 0;
                comboStep = 0;
            }
        }
    }

    public void AttackOver()
    {
        isAttack = false;
    }

    private void UpdateTip() {
        attackSetpTip.text = $"attack {comboStep.ToString()}";
        attackTimerTip.text = $"timer: {attackGapTimer.ToString("f2")}";
    }

    public void GotHurt() {
        isGetHurt = true;
    }

    public void HurtOver()
    {
        isGetHurt = false;
    }
}
