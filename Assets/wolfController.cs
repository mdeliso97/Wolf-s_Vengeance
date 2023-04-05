using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class wolfController : MonoBehaviour
{
    public float moveSpeed = 20;
    public Rigidbody2D rb;

    Vector2 moveDirection;
    public Health health;// =  GetComponent<Health>(); 

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);


    }

    void OnCollisionEnter2D()
    {

        health = GetComponent<Health>();
        health.health--;

        print(health.health);
        if (health.health == 0)
        {
            print("you ded");
            //Time.timeScale = 0;
            SceneManager.LoadScene("MenuScene");
        }

    }
}
