using UnityEngine;
using System.Collections;

public class DeactivateEvent : MonoBehaviour
{
    public GameObject throwPrefab;
    public GameObject realPlane;
    private GameObject instantiatedObject;
    private GameObject Plane;
    private BoxCollider specificCollider;

    // private TextMeshPro textMeshPro
    private void Start()
    {
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
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            // Attach the Rotate.cs script to the instantiated object
            rotate rotateScript = instantiatedObject.GetComponent<rotate>();
            if (rotateScript == null)
            {
                rotateScript = instantiatedObject.AddComponent<rotate>();
            }

            // Pass information of the user's slide to calculate necessary force
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            float forceApplied = touchDeltaPosition.y;
            rotateScript.forceApplied = forceApplied * 0.08f;
            rotateScript.specificCollider = specificCollider;
            instantiatedObject = null;
            StartCoroutine(delayThrow());
        }
    }

    private void SpawnTrash()
    {
        // Calculate the spawn position in front of the XR Origin or middle of the screen
        Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * 0.5f - Camera.main.transform.up * 0.2f; // Adjust the Y value as needed
        Quaternion spawnRotation = Camera.main.transform.rotation;

        // Instantiate the prefab at the calculated position and rotation
        instantiatedObject = Instantiate(throwPrefab, spawnPosition, spawnRotation);
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