using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf_Movements : MonoBehaviour
{
    private float wolfSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        
        transform.position += new Vector3(horizontalInput * wolfSpeed * Time.deltaTime, verticalInput * wolfSpeed * Time.deltaTime, 0f);

        Animator animator = GetComponent<Animator>();
        if (horizontalInput > 0f)
        {
            animator.SetInteger("Direction", 1); // right
        }
        else if (verticalInput > 0f || horizontalInput > 0f)
        {
            animator.SetInteger("Direction", 1); // up + right
        }
        else if (verticalInput < 0f || horizontalInput > 0f)
        {
            animator.SetInteger("Direction", 1); // down + right
        }
        else if (horizontalInput > 0f || horizontalInput < 0f)
        {
            animator.SetInteger("Direction", 0); // left + right
        }
        if (horizontalInput < 0f)
        {
            animator.SetInteger("Direction", 2); // left
        }

        else if (verticalInput > 0f || horizontalInput < 0f)
        {
            animator.SetInteger("Direction", 2); // up + left
        }

        else if (verticalInput < 0f || horizontalInput < 0f)
        {
            animator.SetInteger("Direction", 2); // down + left
        }
        else if (horizontalInput > 0f || horizontalInput < 0f)
        {
            animator.SetInteger("Direction", 0); // idle
        }
        else
        {
            animator.SetInteger("Direction", 0); // idle
        }
    }
}
