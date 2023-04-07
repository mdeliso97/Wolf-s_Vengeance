using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Wolf_isBiting : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject animatedObject;
    public string animationName;
    int key = 0;
    bool isPlaying = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Animator animator = GetComponent<Animator>();

        IEnumerator SetIsBite(int value, float delay)
        {
            yield return new WaitForSeconds(delay);
            animator.SetInteger("isBite", value);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (key == 0 && isPlaying == false)
            {
                isPlaying = true;
                animator.SetBool("isPlaying", isPlaying);
                key = 1;
                StartCoroutine(SetIsBite(key, 0f)); // set isBite to 1 after a 0.1 second delay
            }
            else if (key == 1 && isPlaying == false)
            {
                isPlaying = true;
                animator.SetBool("isPlaying", isPlaying);
                key = 0;
                StartCoroutine(SetIsBite(key, 0f)); // set isBite to 0 after a 0.1 second delay
            }
        }
        else
        {
            isPlaying = false;
            animator.SetBool("isPlaying", isPlaying);
        }
    }
}
