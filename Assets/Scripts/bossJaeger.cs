using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class bossJaeger : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Weapon weapon1;
    public Weapon weapon2;
    public BossHealth health;
    private Animator animator;

    float isShooting = 0f;

    private int biteLayer;

    public bool active = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        biteLayer = LayerMask.NameToLayer("bite");
        InvokeRepeating("chooseAttack", 0, 10);
    }

    void chooseAttack()
    {
        int rand = UnityEngine.Random.Range(0, 3);

        switch (rand)
        {
            case 0:
                StartCoroutine(SpinAttackCoroutine()); break;
            case 1:
                StartCoroutine(DashAttackCoroutine()); break;
            case 2:
                StartCoroutine(CannonAttackCoroutine()); break;
        }

    }

    IEnumerator CannonAttackCoroutine()
    {
        float duration = 2f;
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            weapon2.transform.RotateAround(rb.position, new Vector3(0, 0, 1), Time.deltaTime*10f);

            yield return null;
        }

        weapon1.FireBomb();
        weapon2.FireBomb();
    }

    IEnumerator DashAttackCoroutine()
    {
        int dashCount = 0;
        while (dashCount < 3)
        {
            Vector2 wolfPosition = GameObject.Find("Wolf").transform.position;
            Vector2 aimDirection = wolfPosition - rb.position;
            float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = aimAngle;

            float startTime = Time.time;
            while (Time.time - startTime < 3f)
            {
                aimDirection = wolfPosition - rb.position;
                aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
                rb.rotation = aimAngle;

                rb.position = Vector2.MoveTowards(rb.position, wolfPosition, moveSpeed * Time.deltaTime * 80);
                yield return null;
            }

            dashCount++;
        }

    }

    IEnumerator SpinAttackCoroutine()
    {
        float duration = 8f; 
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            if (isShooting > 0.1)
            {
                weapon1.Fire();
                weapon2.Fire();
                isShooting = 0;
            }

            isShooting += Time.deltaTime;
            transform.Rotate(0, 0, 100*Time.deltaTime);

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == biteLayer)
        {
            health.health--;
            print(health.health);

            if (health.health == 0)
            {
                animator.SetBool("isDead", true);
                StartCoroutine(ExitAfterAnimation());
            }
        }
    }

    private IEnumerator ExitAfterAnimation()
    {
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("MenuScene");
    }
}