using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Respawner : MonoBehaviour
{
    public ARRaycastManager arRaycaster;
    public List<GameObject> placeObjects = new List<GameObject>();
    private List<GameObject> spawnObject = new List<GameObject>();
    private int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach(var obj in placeObjects) {
            Texture2D m_tex = RuntimePreviewGenerator.GenerateModelPreview(GameObject.Find(obj.name).transform);
            byte[] bytes = m_tex.EncodeToPNG();
            var dirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/SaveImages/";
            if(!Directory.Exists(dirPath)) {
                Directory.CreateDirectory(dirPath);
            }
            File.WriteAllBytes(dirPath + "Image_" + obj.name + ".png", bytes);
        }
    }

    // Update is called once per frame
    void Update()
    {   
        //UpdateCenterObject();
        PlaceObjectByTouch();
    }

    public void ChangeRespawnTarget(string args) {
        Int32.TryParse(args, out i);
    }

    private void PlaceObjectByTouch() {
        if(Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began) {
                // 한번만 실행
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if(arRaycaster.Raycast(touch.position, hits, TrackableType.Planes)) {
                    Pose hitPose = hits[0].pose;
                    spawnObject.Add(Instantiate(placeObjects[i], hitPose.position, hitPose.rotation));
                    if(spawnObject.Count == 0) {
                        // spawnObject.Add(Instantiate(placeObjects[j], hitPose.position, hitPose.rotation));
                    } else {
                        // spawnObject.transform.position = hitPose.position;
                        // spawnObject.transform.rotation = hitPose.rotation;
                    }
                }
            }
        }
    }

    public void DestroyAll(string args) {
        foreach(var obj in spawnObject)
            Destroy(obj);
    }

    // private void UpdateCenterObject() {
    //     Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
    //     List<ARRaycastHit> hits = new List<ARRaycastHit>();
    //     arRaycaster.Raycast(screenCenter, hits, TrackableType.Planes);
    //     if(hits.Count > 0) {
    //         Pose placementPose = hits[0].pose;
    //         placeObject.SetActive(true);
    //         placeObject.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
    //     } else {
    //         placeObject.SetActive(false);
    //     }
    // }
}
