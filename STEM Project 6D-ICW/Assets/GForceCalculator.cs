using UnityEngine;

public class GForceCalculator : MonoBehaviour
{
    public Rigidbody aircraftRigidbody;  // De Rigidbody van het vliegtuig
    public float maxTotalGForce = 9.0f;  // Maximale totale G-kracht (9G)
    public float maxVerticalGForce = 6.0f;  // Maximale verticale G-kracht (6G voor loopings)
    public float maxSpeed = 100f;        // Maximale snelheid van het vliegtuig
    public float dragAtMaxG = 5.0f;      // Weerstand die optreedt bij hoge G-krachten
    public float normalDrag = 0.1f;      // Normale luchtweerstand van het vliegtuig
    public float controlSensitivity = 0.2f; // Hoe gevoelig de besturing is
    public float maxRotationSpeed = 50f;    // Maximale rotatiesnelheid in graden per seconde

    private Vector3 previousVelocity;    // Om de snelheid van het vorige frame op te slaan
    private float totalGForce;           // Totale G-kracht (alle richtingen)
    private float verticalGForce;        // Verticale G-kracht (voor loopings)
    private bool isAtMaxTotalG = false;  // Bijhouden of we op de maximale totale G-kracht zitten
    private bool isAtMaxVerticalG = false; // Bijhouden of we op de maximale verticale G-kracht zitten

    void Start()
    {
        if (aircraftRigidbody == null)
        {
            // Automatisch de Rigidbody vinden als deze niet is ingesteld in de Inspector
            aircraftRigidbody = GetComponent<Rigidbody>();
        }

        // De vorige snelheid instellen op de beginsnelheid
        previousVelocity = aircraftRigidbody.velocity;
    }

    void Update()
    {
        CalculateGForce();
        ApplyGForceEffects();
    }

    // Methode om de G-kracht te berekenen
    void CalculateGForce()
    {
        // Bereken de totale versnelling: (snelheidsverandering) / tijd
        Vector3 currentVelocity = aircraftRigidbody.velocity;
        Vector3 acceleration = (currentVelocity - previousVelocity) / Time.deltaTime;

        // Converteer de versnelling naar totale G-kracht (9.81 m/s² is 1G)
        totalGForce = acceleration.magnitude / 9.81f;

        // Bereken de verticale G-kracht (door de projectie van de versnelling op de up-vector van het vliegtuig)
        verticalGForce = Vector3.Dot(acceleration, transform.up) / 9.81f;

        // Sla de huidige snelheid op voor het volgende frame
        previousVelocity = currentVelocity;
    }

    // Methode om de effecten van de G-kracht toe te passen
    void ApplyGForceEffects()
    {
        // Controleer op maximale totale G-kracht
        if (totalGForce >= maxTotalGForce)
        {
            // Limiteer de totale G-kracht tot 9G
            totalGForce = maxTotalGForce;

            if (!isAtMaxTotalG)
            {
                Debug.Log("Pay attention, you are at 9G total G-force!");
                isAtMaxTotalG = true;  // Voorkom dat we dit bericht blijven spammen
            }

            // Verhoog de drag om het vliegtuig langzamer te maken
            aircraftRigidbody.drag = dragAtMaxG;

            // Beperk de snelheid van het vliegtuig
            aircraftRigidbody.velocity = Vector3.ClampMagnitude(aircraftRigidbody.velocity, maxSpeed);

            // Verminder de controle bij hoge totale G-krachten
            ApplySteering(reduced: true);
        }
        else
        {
            isAtMaxTotalG = false;
        }

        // Controleer op maximale verticale G-kracht
        if (Mathf.Abs(verticalGForce) >= maxVerticalGForce)
        {
            // Limiteer de verticale G-kracht tot de opgegeven maximale waarde
            verticalGForce = Mathf.Sign(verticalGForce) * maxVerticalGForce;

            if (!isAtMaxVerticalG)
            {
                Debug.Log("Warning: Vertical G-force at limit: " + verticalGForce.ToString("F2") + "G");
                isAtMaxVerticalG = true;  // Voorkom dat we dit bericht blijven spammen
            }

            // Verminder de controle bij hoge verticale G-krachten
            ApplySteering(reduced: true);
        }
        else
        {
            isAtMaxVerticalG = false;
        }

        // Reset de drag naar normaal als de G-kracht onder de limieten is
        if (!isAtMaxTotalG && !isAtMaxVerticalG)
        {
            aircraftRigidbody.drag = normalDrag;
            ApplySteering(reduced: false);  // Normale besturing
        }

        // Optioneel, toon de huidige G-krachten in de console
        Debug.Log("Current Total G-Force: " + totalGForce.ToString("F2") + "G");
        Debug.Log("Current Vertical G-Force: " + verticalGForce.ToString("F2") + "G");
    }

    // Methode om besturing toe te passen
    void ApplySteering(bool reduced)
    {
        // Lees stuurinvoer van de speler
        float steerInputX = Input.GetAxis("Horizontal");
        float steerInputY = Input.GetAxis("Vertical");

        // Pas de gevoeligheid aan op basis van de G-krachten
        if (reduced)
        {
            steerInputX *= controlSensitivity;
            steerInputY *= controlSensitivity;
        }

        // Rotatievector bepalen op basis van invoer
        Vector3 rotation = new Vector3(-steerInputY, steerInputX, 0f) * controlSensitivity;

        // Voeg torque toe, maar beperk de rotatiesnelheid
        Vector3 currentAngularVelocity = aircraftRigidbody.angularVelocity;
        if (currentAngularVelocity.magnitude < maxRotationSpeed * Mathf.Deg2Rad)  // Omzetten naar radialen
        {
            aircraftRigidbody.AddTorque(rotation, ForceMode.Acceleration);
        }
    }
}
