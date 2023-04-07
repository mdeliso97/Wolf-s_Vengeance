using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public float cameraSpeed = 20f;

    // Update is called once per frame
    void Update()
    {
        float move_x = Input.GetAxis("Horizontal");
        float move_y = Input.GetAxis("Vertical");

        transform.Translate(move_x * Time.deltaTime * cameraSpeed, move_y * Time.deltaTime * cameraSpeed, 0);
    }
}
