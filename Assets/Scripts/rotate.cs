using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class rotate : MonoBehaviour
{
    public float minForce = 1f;
    public float maxForce = 5f;
    public float forceApplied = 5f;
    public float dampingFactor = 0.9f; // Damping factor to reduce force
    public float rotationSpeed = 90f;
    public float slideAngle = 0f;
    public float windForce = 1f;

    Rigidbody rb;
    ConstantForce wind;
    AudioSource audioSource;

    public BoxCollider specificCollider = null;

    private bool isRotating = true;
    private Vector3 rotationAxis;
    private GameObject arrow;
    private float windAngle = 0;
    private float[] timeSound = { 1.3f, 8.3f, 12.5f, 15.5f, 19f, 24.3f, 29.8f, 32.9f };

    void Start()
    {
        Debug.Log(forceApplied);
        if (forceApplied < minForce)
        {
            forceApplied = minForce;
        }
        else if (forceApplied > maxForce)
        {
            forceApplied = maxForce;
        }
        arrow = GameObject.FindGameObjectWithTag("Wind");
        Quaternion rotation = arrow.transform.rotation;
        windAngle = rotation.eulerAngles.z * -1;
        Debug.Log("WindAngle: " + windAngle.ToString());

        gameObject.AddComponent<Rigidbody>();
        //  gameObject.AddComponent<BoxCollider>();
        gameObject.AddComponent<destroyAfterCertainSeconds>();
        gameObject.AddComponent<ConstantForce>();

        rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.mass = 0.1f;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        wind = gameObject.GetComponent<ConstantForce>();

        audioSource = gameObject.GetComponent<AudioSource>();

        wind.force = Quaternion.Euler(0f, windAngle, 0f) * Camera.main.transform.forward.normalized * windForce;

        Vector3 slideDirection = Quaternion.Euler(0f, slideAngle, 0f) * Camera.main.transform.forward.normalized;
        rb.AddForce(slideDirection * forceApplied, ForceMode.Impulse);
        rotationAxis = Quaternion.Euler(0f, slideAngle, 0f) *  Vector3.right.normalized;
        rotationSpeed *= forceApplied;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isRotating)
        {
            rotationSpeed *= dampingFactor;
            // windForce *= dampingFactor;
            if (rotationSpeed < 30f) rotationSpeed = 0;
            // if (windForce < 0.1f) windForce = 0;
        }
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
        // wind.force = Quaternion.Euler(0f, windAngle, 0f) * Camera.main.transform.forward.normalized * windForce;

    }
    void FixedUpdate()
    {
        rb.velocity *= dampingFactor; // Apply damping factor to reduce force
        wind.force *= dampingFactor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == specificCollider)
        {
            // Handle collision with the specific collider
            Debug.Log("Score");
            Debug.Log("Trigger collision with specific collider detected");
            // mySource.Play();
        }
        else
        {
            Debug.Log("Trigger collision with other collider detected");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Stop rotating when a collision occurs
        isRotating = false;
        int choice = UnityEngine.Random.Range(0, timeSound.Length);
        audioSource.time = timeSound[choice];
        audioSource.Play();
        Debug.Log("Collider Sound");
        StartCoroutine(StopSound());
    }
    IEnumerator StopSound()
    {
        yield return new WaitForSeconds(0.2f);
        audioSource.Stop();
    }

}