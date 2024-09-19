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

        // Calculate the bank angle accurately
        float bankAngle = CalculateBankAngle();
        Debug.Log("Bank Angle: " + bankAngle.ToString("F2") + " degrees");

        // Convert bank angle to a range of 0 to 1 (for angles between 0 and 90 degrees)
        float normalizedBank = Mathf.Clamp01(Mathf.Abs(bankAngle) / 90f);

        // Apply coordinated yaw torque based on bank angle
        float turnForce = normalizedBank * Mathf.Sign(roll) * rolleffectivness;
        rb.AddTorque(transform.up * turnForce * 250);

        // Apply lift force proportional to the velocity and lift coefficient
        rb.AddForce(Vector3.up * rb.velocity.magnitude * lift);
    }

    private float CalculateBankAngle()
    {
        // Calculate bank angle between the plane's right vector and the horizontal plane
        Vector3 right = transform.right;
        right.y = 0; // Project onto the horizontal plane
        right.Normalize(); // Normalize to ensure a unit vector

        float bankAngle = Vector3.SignedAngle(Vector3.right, right, transform.forward);
        return bankAngle;
    }

    private void UpdateHUD()
    {
        hud.text = "Throttle: " + throttle.ToString("F0") + "%\n";
        hud.text += "Airspeed: " + ((rb.velocity.magnitude * 3.6f) / 1.852).ToString("F0") + " KIAS\n";
        hud.text += "Altitude: " + transform.position.y.ToString("F0") + "ft AMSL";
    }
}
