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

    public float hitCooldownTime = 2f;

    private int bulletLayer;
    private float hitTime = 0f;
    private bool isHit = false;

    private SpriteRenderer mSpriteRenderer;
    private BoxCollider2D mBoxCollider;

    void Start() {
        health = GetComponent<Health>();
        bulletLayer = LayerMask.NameToLayer("bullet");
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        mBoxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);

        if (isHit) {
            hitTime += Time.deltaTime;

            if (hitTime >= hitCooldownTime) {
                isHit = false;
                hitTime = 0f;
                mBoxCollider.excludeLayers = LayerMask.GetMask();
                mSpriteRenderer.color = new Color(mSpriteRenderer.color.r, mSpriteRenderer.color.g, mSpriteRenderer.color.b, 1f);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isHit && collision.gameObject.layer == bulletLayer) {
            health.health--;

            isHit = true;
            mBoxCollider.excludeLayers = LayerMask.GetMask("bullet");
            mSpriteRenderer.color = new Color(mSpriteRenderer.color.r, mSpriteRenderer.color.g, mSpriteRenderer.color.b, 0.2f);

            print(health.health);
            if (health.health == 0)
            {
                print("you ded");
                //Time.timeScale = 0;
                SceneManager.LoadScene("MenuScene");
            }
        }
    }

}
