using UnityEngine;
public class rotate : MonoBehaviour
{
    public float minForce = 1f;
    public float maxForce = 8;
    public float forceApplied = 5;
    public float dampingFactor = 0.8f; // Damping factor to reduce force
    public float rotationSpeed = 600f;
    Rigidbody rb;
    public BoxCollider specificCollider = null;

    private bool isRotating = true;
    private Vector3 rotationAxis;


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
        gameObject.AddComponent<Rigidbody>();
        gameObject.AddComponent<BoxCollider>();
        gameObject.AddComponent<destroyAfterCertainSeconds>();

        rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.mass = 0.1f;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        rb.AddForce(Camera.main.transform.forward.normalized * forceApplied, ForceMode.Impulse);
        rotationAxis = Vector3.right;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isRotating)
        {
            // Rotate the object around its up axis (Y-axis) by a small amount each frame
            transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
        }
    }
    void FixedUpdate()
    {
        rb.velocity *= dampingFactor; // Apply damping factor to reduce force
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
    }

}