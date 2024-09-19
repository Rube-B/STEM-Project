using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("An array of transforms representing camera positions")]
    [SerializeField] Transform[] povs;
    
    [Tooltip("The speed at which the camera follows the plane")]
    [SerializeField] float speed = 200000f;
    
    [Tooltip("Offset from the plane's position to avoid camera lagging behind")]
    [SerializeField] Vector3 offset = new Vector3(0, 0, 0);
    
    [Tooltip("Smooth time for the camera movement")]
    [SerializeField] float smoothTime = 0.3f;

    private int index = 1;
    private Vector3 target;
    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        // Change camera POV based on key input
        if (Input.GetKeyDown(KeyCode.Alpha1)) index = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) index = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) index = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha4)) index = 3;
        else if (Input.GetKeyDown(KeyCode.Alpha5)) index = 4;

        // Set the target position with an offset
        target = povs[index].position + offset;
    }

    private void FixedUpdate()
    {
        // Smoothly move the camera to the target position
        transform.position = Vector3.MoveTowards(transform.position, target, 500000);//ref velocity, smoothTime

        // Make the camera look in the same direction as the plane
        transform.forward = Vector3.Lerp(transform.forward, povs[index].forward, Time.deltaTime * speed);
    }
}
