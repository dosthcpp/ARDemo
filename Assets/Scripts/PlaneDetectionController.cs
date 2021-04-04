using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class PlaneDetectionController : MonoBehaviour
{
    ARPlaneManager m_ARPlaneManager;

    private void Start() {
        
    }

    void Awake()
    {
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
    }

    private void Update() {
        foreach (var plane in m_ARPlaneManager.trackables)
            plane.gameObject.SetActive(false);
    }
}