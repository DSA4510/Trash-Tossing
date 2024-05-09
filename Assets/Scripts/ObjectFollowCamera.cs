using UnityEngine;
using Unity.XR.CoreUtils;
using TMPro;
using UnityEditor.XR.LegacyInputHelpers;

public class ObjectFollowCamera : MonoBehaviour
{
    private Transform xrOrigin;
    void Start()
    {
        xrOrigin = GameObject.FindWithTag("XROrigin").transform;
    }
}