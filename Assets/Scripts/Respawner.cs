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
    
    [SerializeField]
    private Camera arCamera;
    private Vector3 curScale;
    private float yLoc, yAngle;
    private float prevAngle = 0.0f;
    private bool isSelect = false;
    private int i = 0, selected = 9999;

    private UnityMessageManager Manager {
        get {return GetComponent<UnityMessageManager>(); }
        // Manager.SendMessageToFlutter(Convert.ToBase64String(fileData));
    }

    // Start is called before the first frame update
    void Start()
    {
        // foreach(var obj in placeObjects) {
        for(var i = 0 ; i < placeObjects.Count; ++i) {
            Texture2D m_tex = RuntimePreviewGenerator.GenerateModelPreview(GameObject.Find(placeObjects[i].name).transform);
            byte[] bytes = m_tex.EncodeToPNG();
            Manager.SendMessageToFlutter(Convert.ToBase64String(bytes));
            // var dirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "SaveImages/";
            // if(!Directory.Exists(dirPath)) {
            //     Directory.CreateDirectory(dirPath);
            // }
            // File.WriteAllBytes(dirPath + "Image_" + obj.name + ".png", bytes);
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
            if(!isSelect) {
                if(touch.phase == TouchPhase.Began) {
                // 한번만 실행
                    List<ARRaycastHit> hits = new List<ARRaycastHit>();
                    if(arRaycaster.Raycast(touch.position, hits, TrackableType.Planes)) {
                        Pose hitPose = hits[0].pose;
                        spawnObject.Add(Instantiate(placeObjects[i], hitPose.position, Quaternion.Euler(hitPose.rotation.x - 90.0f, hitPose.rotation.y, hitPose.rotation.z) ));
                        
                        if(spawnObject.Count == 0) {
                            // spawnObject.Add(Instantiate(placeObjects[j], hitPose.position, hitPose.rotation));
                        } else {
                            // spawnObject.transform.position = hitPose.position;
                            // spawnObject.transform.rotation = hitPose.rotation;
                        }
                    }
                }
            } else {
                if (selected != 9999 && touch.phase == TouchPhase.Moved) {
                    List<ARRaycastHit> hits = new List<ARRaycastHit>();
                    if(arRaycaster.Raycast(touch.position, hits, TrackableType.Planes)) {
                        Pose hitPose = hits[0].pose;
                        // y축은 그대로 냅둔다
                        spawnObject[selected].transform.position = new Vector3(hitPose.position.x, spawnObject[selected].transform.position.y, hitPose.position.z);
                    }
                }
            }
        }
    }

    public void ReleaseSelected(string args) {
        isSelect = false;
    }

    public void SelectObject(string args) {
        var x = Screen.width / 2;
        var y = Screen.height / 2;

        Ray ray;
        RaycastHit hitobj;

        ray = arCamera.ScreenPointToRay(new Vector3(x, y, 0));
        if(Physics.Raycast(ray, out hitobj)) {
            var i = 0;
            for(; i < spawnObject.Count && !(hitobj.transform.GetInstanceID() == spawnObject[i].transform.GetInstanceID()); ++i);
            if(i < spawnObject.Count) {
                // if(prevMaterial && (prev >= 0 && prev < spawnObject.Count)) {
                //     spawnObject[prev].transform.GetComponent<MeshRenderer>().material = prevMaterial;
                // }
                // 색깔 바꾸기전에 prev에 저장
                // prevMaterial = spawnObject[i].transform.GetComponent<MeshRenderer>().material;
                // spawnObject[i].transform.GetComponent<MeshRenderer>().material.color = Color.red;
                selected = i;
                isSelect = true;
                curScale = spawnObject[selected].transform.localScale;
                yLoc = spawnObject[selected].transform.position.y;
                prevAngle = 0.0f;
                Manager.SendMessageToFlutter("!" + spawnObject[selected].name + "selected");
            } else {
                // if(selected >= 0 && selected < spawnObject.Count) {
                //     spawnObject[selected].transform.GetComponent<MeshRenderer>().material = prevMaterial;
                // }
            }
        } else {
            Manager.SendMessageToFlutter("!missing");
            isSelect = false;
            curScale = new Vector3();
            prevAngle = 0.0f;
            selected = 9999;
        }
    }

    public void Resize(string args) {
        if(selected != 9999 && curScale != null) {
            spawnObject[selected].transform.localScale = curScale * float.Parse(args);
        }
    }

    public void Rotate(string args) {
        if(selected != 9999) {
            // spawnObject[selected].transform.RotateAround(spawnObject[selected].transform.position, spawnObject[selected].transform.forward, yAngle + float.Parse(args));
            if(float.Parse(args) - prevAngle > 0) { // increasing
                spawnObject[selected].transform.Rotate(new Vector3(0, 0, 1));
            } else { //  decreasing
                spawnObject[selected].transform.Rotate(new Vector3(0, 0, -1));
            }
            prevAngle = float.Parse(args);
        }

    }

    public void AdjustHeight(string args) {
        if(selected != 9999) {
            spawnObject[selected].transform.position = new Vector3(spawnObject[selected].transform.position.x, yLoc + float.Parse(args), spawnObject[selected].transform.position.z);
        }
    }

    public void DestroySelected(string args) {
        if(selected != 9999) {
            var obj = spawnObject[selected];
            spawnObject.RemoveAt(selected);
            Destroy(obj);
        }
    }

    public void DestroyAll(string args) {
        foreach(var obj in spawnObject)
            Destroy(obj);
        spawnObject.Clear();
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
