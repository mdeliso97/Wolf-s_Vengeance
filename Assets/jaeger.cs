using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class NewBehaviourScript : MonoBehaviour
{
    public float moveSpeed = 1f;
    public Rigidbody2D rb;
    public Weapon weapon;
    public float walkingDistance = 12;

    Vector2 wolfPosition;
    Vector2 selfPosition;

    float isShooting = 0f;
    bool isWalking = true;
    float distanceToWolf = 0f;


    // Update is called once per frame
    void Update()
    {
        wolfPosition = GameObject.Find("Wolf").transform.position;
        selfPosition = transform.position;
        distanceToWolf = Vector2.Distance(selfPosition, wolfPosition);//Mathf.Abs((wolfPosition - selfPosition).magnitude);

        if (distanceToWolf > walkingDistance)
        {
            isWalking = true;

        } else
        {
            isWalking = false;
        }

        if(isShooting>3 && ! isWalking)
        {
            weapon.Fire();
            isShooting = 0;
        }
        isShooting += Time.deltaTime;

        //if (isWalking)
        //{

            Vector2 aimDirection = wolfPosition - rb.position;
            float d = Vector2.Distance(wolfPosition, rb.position);
            transform.position = Vector2.MoveTowards(selfPosition, wolfPosition - aimDirection.normalized * 
                (walkingDistance -2) , moveSpeed*Time.deltaTime*d);

            //rb.AddForce(new Vector2(aimDirection.x * moveSpeed, aimDirection.y * moveSpeed));
            //rb.velocity = new Vector2(aimDirection.x * moveSpeed, aimDirection.y * moveSpeed);
        //}
        //else
        //{
          //  rb.velocity = new Vector2(0, 0);

        //}




    }
    private void FixedUpdate()
    {

        Vector2 aimDirection = wolfPosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;


    }

}
