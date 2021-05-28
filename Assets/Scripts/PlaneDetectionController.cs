using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class PlaneDetectionController : MonoBehaviour
{
    [SerializeField]
    private Material grassMat, groundMat;
    private bool ground;

    ARPlaneManager m_ARPlaneManager;

    private bool showTexture = true;

    private void Start() {
        grassMat = (Material)Resources.Load("ARPlane", typeof(Material));
        groundMat = (Material)Resources.Load("ARPlane2", typeof(Material));
    }

    public void SetTexture(string args) {
        showTexture = !showTexture;
    }

    public void ChangeTexture(string args) {
        ground = !ground;
        foreach(var plane in m_ARPlaneManager.trackables) {
            if(!ground) {
                plane.gameObject.GetComponent<MeshRenderer>().material = grassMat;
            } else {
                plane.gameObject.GetComponent<MeshRenderer>().material = groundMat;
            }
        }
        if(!ground) {
            GameObject.Find("ARPlane").GetComponent<MeshRenderer>().material = grassMat;
        } else {
            GameObject.Find("ARPlane").GetComponent<MeshRenderer>().material = groundMat;
        }
    }

    void Awake()
    {
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
    }

    private void Update() {
        foreach (var plane in m_ARPlaneManager.trackables) {
            plane.gameObject.SetActive(showTexture);
        }
    }
}