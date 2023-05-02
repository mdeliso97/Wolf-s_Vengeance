using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;
using UnityEngine.UIElements;

public class bossJaeger : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Weapon weapon1;
    public Weapon weapon2;

    float isShooting = 0f;

    void Start()
    {
        InvokeRepeating("chooseAttack", 0, 10);
    }

    void chooseAttack()
    {
        int rand = UnityEngine.Random.Range(0, 3); // Generate a random integer either 0 or 1
        print(rand);

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
        return null;
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
}