using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Wolf_isBiting : MonoBehaviour
{
    public GameObject animatedObject;
    public string animationName;
    public AudioSource bite0Audio;
    public AudioSource bite1Audio;
    private int key = 0;
    private bool isPlaying = false;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        bite0Audio = GameObject.Find("Bite0Sound").GetComponent<AudioSource>();
        bite1Audio = GameObject.Find("Bite1Sound").GetComponent<AudioSource>();
    }

    void Update()
    {
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
                bite1Audio.Play();
            }
            else if (key == 1 && isPlaying == false)
            {
                isPlaying = true;
                animator.SetBool("isPlaying", isPlaying);
                key = 0;
                StartCoroutine(SetIsBite(key, 0f)); // set isBite to 0 after a 0.1 second delay
                bite0Audio.Play();
            }
        }
        else
        {
            isPlaying = false;
            animator.SetBool("isPlaying", isPlaying);
        }
    }
}
