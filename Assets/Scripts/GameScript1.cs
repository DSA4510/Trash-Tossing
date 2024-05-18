using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem.EnhancedTouch;
using System.Diagnostics.Tracing;

public class DeactivateEvent : MonoBehaviour
{
    public GameObject throwPrefab;
    public GameObject realPlane;
    private GameObject instantiatedObject;
    private GameObject Plane;
    private BoxCollider specificCollider;

    private UnityEngine.Touch touch;
    private Vector2 pos_begin;
    private Vector2 pos_end;
    // private TextMeshPro textMeshPro
    private void Start()
    {
        gameObject.tag = "Bin";
        BoxCollider[] boxColliders = GetComponentsInChildren<BoxCollider>();

        // Select the last Box Collider component
        if (boxColliders.Length > 0)
        {
            specificCollider = boxColliders[boxColliders.Length - 1];
            Debug.Log(boxColliders.Length.ToString());
        }
        else
        {
            Debug.Log("Object don't have collider box");
        }
        DisableAndReplaceObjectsByTag("ARPlane");
        SpawnTrash();
    }

    void Update()
    {
        if (instantiatedObject != null)
        {
            // Move the instantiated object to the desired position
            instantiatedObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.5f - Camera.main.transform.up * 0.2f; // Adjust the Y value as needed
            instantiatedObject.transform.rotation = Camera.main.transform.rotation;
        }

        // Check if the player slides their phone
        touch = Input.GetTouch(0);
        TouchPhase phase = touch.phase;
        switch (phase)
        {
            case TouchPhase.Began:
                pos_begin = touch.position;
                break;
            case TouchPhase.Ended:
                pos_end = touch.position;
                Vector2 touchDeltaPosition = pos_end - pos_begin;
                rotate rotateScript = instantiatedObject.GetComponent<rotate>();
                if (rotateScript == null)
                {
                    rotateScript = instantiatedObject.AddComponent<rotate>();
                }

                // Pass information of the user's slide to calculate necessary force
                Vector3 slideDirection = new Vector3(touchDeltaPosition.x, 0f, touchDeltaPosition.y).normalized;
                // Calculate the angle between the camera's forward vector and the slide direction
                float slideAngle = Vector3.SignedAngle(Camera.main.transform.forward, slideDirection, Vector3.up);
                slideAngle = Mathf.Clamp(slideAngle, -90f, 90f) * (1f/6); // Clamp the angle within -60 to 60 degrees

                float forceApplied = touchDeltaPosition.sqrMagnitude;
                Debug.Log("Force: " + (forceApplied*0.00001f).ToString());
                rotateScript.forceApplied = forceApplied * 0.00001f;
                rotateScript.specificCollider = specificCollider;
                rotateScript.slideAngle = slideAngle;

                instantiatedObject = null;
                StartCoroutine(delayThrow());
                break;
        }
    }

    private void SpawnTrash()
    {
        // Calculate the spawn position in front of the XR Origin or middle of the screen
        Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * 0.5f - Camera.main.transform.up * 0.2f; // Adjust the Y value as needed
        Quaternion spawnRotation = Camera.main.transform.rotation;

        // Instantiate the prefab at the calculated position and rotation
        instantiatedObject = Instantiate(throwPrefab, spawnPosition, spawnRotation);
        Debug.Log("Throw");
    }

    IEnumerator delayThrow()
    {
        yield return new WaitForSeconds(1.5f);
        SpawnTrash();
    }

    private void DisableAndReplaceObjectsByTag(string tag)
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obj in objectsWithTag)
        {
            obj.SetActive(false);
        }
        Vector3 planePosition = gameObject.transform.position;
        planePosition.y -= 0.01f;
        Quaternion planeRotation = gameObject.transform.rotation;
        Plane = Instantiate(realPlane, planePosition, planeRotation);
    }
}