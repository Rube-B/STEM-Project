using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    [Header("Plane Stats")]
    [Tooltip("How much the throttle ramps up or down.")]
    public float throttleIncrement = 0.1f;
    [Tooltip("Maximum engine thrust when at 100% throttle.")]
    public float maxThrust = 200f;
    [Tooltip("How responsive the plane is when rolling, pitching, and yawing.")]
    public float responsivness = 10f;

    private float throttle;
    private float roll;
    private float pitch;
    private float yaw;

    private float responseModifier
    {
        get
        {
            return (rb.mass / 10f) * responsivness;
        }
    }
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void HandleInput()
    {
        roll = Input.GetAxis("Roll");
        pitch = Input.GetAxis("Pitch");
        yaw = Input.GetAxis("Yaw");


        if (Input.GetKey(KeyCode.Space))
        {
            throttle += throttleIncrement;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            throttle -= throttleIncrement;
        }
        throttle = Mathf.Clamp(throttle, 0f, 100f);

        
    }
    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * maxThrust * throttle);
        rb.AddTorque(transform.up * yaw * responseModifier);
        rb.AddTorque(transform.right * pitch * responseModifier);
        rb.AddTorque(transform.forward * roll * responseModifier);
    }

}
