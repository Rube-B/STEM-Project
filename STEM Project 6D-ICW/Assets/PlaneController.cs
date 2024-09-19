using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaneController : MonoBehaviour
{
    [Header("Plane Stats")]

    [Tooltip("How much the throttle ramps up or down.")]
    public float throttleIncrement = 0.1f;
    [Tooltip("Maximum engine thrust when at 100% throttle.")]
    public float maxThrust = 200f;
    [Tooltip("How responsive the plane is when rolling, pitching, and yawing.")]
    public float responsivness = 10f;
    [SerializeField] int rolleffectivness;

    public float lift = 135f;

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
    [SerializeField] TextMeshProUGUI hud;

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
        UpdateHUD();
    }

    private void FixedUpdate()
    {
        // Apply thrust force
        rb.AddForce(transform.forward * maxThrust * throttle);

        // Apply roll, pitch, and yaw torques
        rb.AddTorque(transform.up * yaw * responseModifier);
        rb.AddTorque(transform.right * pitch * responseModifier);
        rb.AddTorque(transform.forward * roll * responseModifier);

        // Apply additional roll torque for responsiveness
        rb.AddTorque(transform.right * roll * rolleffectivness);

        // Calculate the bank angle
        float bankAngle = Vector3.Dot(transform.right, Vector3.up);

        // Apply coordinated yaw torque based on bank angle
        float turnForce = bankAngle * Mathf.Abs(roll) * rolleffectivness;
        rb.AddTorque(transform.up * turnForce);

        // Apply lift force proportional to the velocity and lift coefficient
        rb.AddForce(Vector3.up * rb.velocity.magnitude * lift);
    }

    private void UpdateHUD()
    {
        hud.text = "Throttle: " + throttle.ToString("F0") + "%\n";
        hud.text += "Airspeed: " + ((rb.velocity.magnitude * 3.6f) / 1.852).ToString("F0") + " KIAS\n";
        hud.text += "Altitude: " + transform.position.y.ToString("F0") + "ft AMSL";
    }
}
