using System;
using System.IO;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Respawner : MonoBehaviour
{
    [SerializeField]
    Camera arCamera;
    [SerializeField]
    GameObject arSessionOrigin;
    [SerializeField]

    ARRaycastManager arRaycaster;
    [SerializeField]
    Material mat1, mat2, mat3, mat4, mat5, mat6;
    GameObject selectedObj;
    private Vector3 curScale;
    private float yLoc, yAngle;
    private float prevAngle = 0.0f;
    private bool isSelect = false;
    private int respawnIndex = 0;
    private bool firstInstantiated = false;
    private Text downloadProgress;
    DefaultPool pool;
    AssetBundle bundle;

    private List<String> prefabNames = new List<String> {
        "Acacia 1",
        "Acacia 2",
        "Acacia 3",
        "Acacia 4",
        "Acacia 5",
        "Beech 1",
        "Beech 2",
        "Birch 1",
        "Birch 2",
        "Birch 3",
        "bush_dome_01",
        "bush_sphere",
        "Date_Palm",
        "Dry Tree 1",
        "Dry Tree 2",
        "Fallen_Leaves",
        "Fir 1",
        "Fir 2",
        "Fir 3",
        "Fir 4",
        "hedge_003",
        "Ivy_A",
        "Ivy_B",
        "Ivy_C",
        "Juniper 1",
        "Juniper 2",
        "Maple 1",
        "Maple 2",
        "Maple_Leaf",
        "Oak 1",
        "Oak 2",
        "Pine 1",
        "Pine 2",
        "Pine 3",
        "Pine 4",
        "Pine 5",
        "plants_plane",
        "Spruce 1",
        "Spruce 2",
        "Sycamore",
        "tree",
        "Tree_A",
        "Tree_B",
        "tree_cube",
        "tree_sphere",
        "BirdOfParadise_Flower",
        "BlueMink1-mobile",
        "BlueMink2-mobile",
        "BlueMink3-mobile",
        "BlueMink4-mobile",
        "BlueMink5-mobile",
        "Boston_Fern",
        "Cosmos1A-bloomer-mobile",
        "Cosmos1A-mobile",
        "Cosmos1B-bloomer-mobile",
        "Cosmos1B-mobile",
        "Cosmos1C-bloomer-mobile",
        "Cosmos1C-mobile",
        "Cosmos2A-bloomer-mobile",
        "Cosmos2A-mobile",
        "Cosmos2B-bloomer-mobile",
        "Cosmos2B-mobile",
        "Cosmos2C-bloomer-mobile",
        "Cosmos2C-mobile",
        "Daisy-group1-mobile",
        "Daisy-group1bloomer-mobile",
        "Daisy-group2-mobile",
        "Daisy-group2bloomer-mobile",
        "Daisy-group3-mobile",
        "Daisy-group3bloomer-mobile",
        "Daisy-group4-mobile",
        "Daisy-group4bloomer-mobile",
        "Daisy1-mobile",
        "Daisy1bloomer-mobile",
        "Daisy2-mobile",
        "Daisy2bloomer-mobile",
        "Daisy3-mobile",
        "Daisy3bloomer-mobile",
        "Daisy4-mobile",
        "Gazania1A-bloomer-mobile",
        "Gazania1A-mobile",
        "Gazania1B-bloomer-mobile",
        "Gazania1B-mobile",
        "Gazania1C-bloomer-mobile",
        "Gazania1C-mobile",
        "Gazania2A-bloomer-mobile",
        "Gazania2A-mobile",
        "Gazania2B-bloomer-mobile",
        "Gazania2B-mobile",
        "Gazania2C-bloomer-mobile",
        "Gazania2C-mobile",
        "Gazania3A-bloomer-mobile",
        "Gazania3A-mobile",
        "Gazania3B-bloomer-mobile",
        "Gazania3B-mobile",
        "Gazania3C-bloomer-mobile",
        "Gazania3C-mobile",
        "Hollyhock1A-mobile",
        "Hollyhock1B-mobile",
        "Hollyhock1C-mobile",
        "Hollyhock1D-mobile",
        "Hollyhock2A-mobile",
        "Hollyhock2B-mobile",
        "Hollyhock2C-mobile",
        "Hollyhock2D 1",
        "Hollyhock3A-mobile",
        "Hollyhock3B-mobile",
        "Hollyhock3C-mobile",
        "Hollyhock3D-mobile",
        "MorningGlory1A-mobile",
        "MorningGlory1Abloomer-mobile",
        "MorningGlory1B-mobile",
        "MorningGlory1Bbloomer-mobile",
        "MorningGlory2A-mobile",
        "MorningGlory2Abloomer-mobile",
        "MorningGlory2B-mobile",
        "MorningGlory2Bbloomer-mobile",
        "Narcissus1A-mobile",
        "Narcissus1Abloomer-mobile",
        "Narcissus1B-mobile",
        "Narcissus1Bbloomer-mobile",
        "Narcissus2A-mobile",
        "Narcissus2Abloomer-mobile",
        "Narcissus2B-mobile",
        "Narcissus2Bbloomer 1",
        "Narcissus3A-mobile",
        "Narcissus3Abloomer-mobile",
        "Narcissus3B-mobile",
        "Narcissus3Bbloomer-mobile",
        "Palm_Bush",
        "Pampass_Grass",
        "Peperomia1-mobile",
        "Peperomia2-mobile",
        "Peperomia3-mobile",
        "Plant 1",
        "Plant 2",
        "Plant 3",
        "Plant 4",
        "Plant 5",
        "Plant 6",
        "Plant 7",
        "Plant 8",
        "Plant 9",
        "Plant 10",
        "Plant 11",
        "Plant 12",
        "Plant 13",
        "Plant 14",
        "Plant 15",
        "Plant 16",
        "Plant 17",
        "Plant 18",
        "Plant 19",
        "Potted_Yukka",
        "PrimroseAcaulis1A-mobile",
        "PrimroseAcaulis1B-mobile",
        "PrimroseAcaulis1C-mobile",
        "PrimroseAcaulis1D-mobile",
        "PrimroseAcaulis1E-mobile",
        "PrimroseAcaulis1F-mobile",
        "PrimroseAcaulis1G-mobile",
        "PrimroseAcaulis2A-mobile",
        "PrimroseAcaulis2B-mobile",
        "PrimroseAcaulis2C-mobile",
        "PrimroseAcaulis2D-mobile",
        "PrimroseAcaulis2E-mobile",
        "PrimroseAcaulis2F-mobile",
        "PrimroseAcaulis2G-mobile",
        "PrimroseObconica1A-mobile",
        "PrimroseObconica1B-mobile",
        "PrimroseObconica1C-mobile",
        "PrimroseObconica1D-mobile",
        "PrimroseObconica2A-mobile",
        "PrimroseObconica2B-mobile",
        "PrimroseObconica2C-mobile",
        "PrimroseObconica2D-mobile",
        "Saintpaulia1A-mobile",
        "Saintpaulia1B-mobile",
        "Saintpaulia1C-mobile",
        "Saintpaulia2A-mobile",
        "Saintpaulia2B-mobile",
        "Saintpaulia2C-mobile",
        "Saintpaulia3A-mobile",
        "Saintpaulia3B-mobile",
        "Saintpaulia3C-mobile",
        "Strawberry1-mobile",
        "Strawberry2-mobile",
        "Strawberry3-mobile",
        "Tulipa-group1A-bloomer-mobile",
        "Tulipa-group1A-mobile",
        "Tulipa-group1B-bloomer-mobile",
        "Tulipa-group1B-mobile",
        "Tulipa-group1C-bloomer-mobile",
        "Tulipa-group1C-mobile",
        "Tulipa-group1D-bloomer-mobile",
        "Tulipa-group1D-mobile",
        "Tulipa-group1E-bloomer-mobile",
        "Tulipa-group1E-mobile",
        "Tulipa-group1F-bloomer-mobile",
        "Tulipa-group1F-mobile",
        "Tulipa-group1G-bloomer-mobile",
        "Tulipa-group1G-mobile",
        "Tulipa-group1H-bloomer-mobile",
        "Tulipa-group1H-mobile",
        "Tulipa-group1I-bloomer-mobile",
        "Tulipa-group1I-mobile",
        "Tulipa-group2A-bloomer-mobile",
        "Tulipa-group2A-mobile",
        "Tulipa-group2B-bloomer-mobile",
        "Tulipa-group2B-mobile",
        "Tulipa-group2C-bloomer-mobile",
        "Tulipa-group2C-mobile",
        "Tulipa-group2D-bloomer-mobile",
        "Tulipa-group2D-mobile",
        "Tulipa-group2E-bloomer-mobile",
        "Tulipa-group2E-mobile",
        "Tulipa-group2F-bloomer-mobile",
        "Tulipa-group2F-mobile",
        "Tulipa-group2G-bloomer-mobile",
        "Tulipa-group2G-mobile",
        "Tulipa-group2H-bloomer-mobile",
        "Tulipa-group2H-mobile",
        "Tulipa-group2I-bloomer-mobile",
        "Tulipa-group2I-mobile",
        "Tulipa-group3A-bloomer-mobile",
        "Tulipa-group3A-mobile",
        "Tulipa-group3B-bloomer-mobile",
        "Tulipa-group3B-mobile",
        "Tulipa-group3C-bloomer-mobile",
        "Tulipa-group3C-mobile",
        "Tulipa-group3D-bloomer-mobile",
        "Tulipa-group3D-mobile",
        "Tulipa-group3E-bloomer-mobile",
        "Tulipa-group3E-mobile",
        "Tulipa-group3F-bloomer-mobile",
        "Tulipa-group3F-mobile",
        "Tulipa-group3G-bloomer-mobile",
        "Tulipa-group3G-mobile",
        "Tulipa-group3H-bloomer-mobile",
        "Tulipa-group3H-mobile",
        "Tulipa-group3I-bloomer-mobile",
        "Tulipa-group3I-mobile",
        "Tulipa1A-bloomer-mobile",
        "Tulipa1A-mobile",
        "Tulipa1B-bloomer-mobile",
        "Tulipa1B-mobile",
        "Tulipa1C-bloomer-mobile",
        "Tulipa1C-mobile",
        "Tulipa1D-bloomer-mobile",
        "Tulipa1D-mobile",
        "Tulipa1E-bloomer-mobile",
        "Tulipa1E-mobile",
        "Tulipa1F-bloomer-mobile",
        "Tulipa1F-mobile",
        "Tulipa1G-bloomer-mobile",
        "Tulipa1G-mobile",
        "Tulipa1H-bloomer-mobile",
        "Tulipa1H-mobile",
        "Tulipa1I-bloomer-mobile",
        "Tulipa1I-mobile",
        "Tulipa2A-bloomer-mobile",
        "Tulipa2A-mobile",
        "Tulipa2B-bloomer-mobile",
        "Tulipa2B-mobile",
        "Tulipa2C-bloomer-mobile",
        "Tulipa2C-mobile",
        "Tulipa2D-bloomer-mobile",
        "Tulipa2D-mobile",
        "Tulipa2E-bloomer-mobile",
        "Tulipa2E-mobile",
        "Tulipa2F-bloomer-mobile",
        "Tulipa2F-mobile",
        "Tulipa2G-bloomer-mobile",
        "Tulipa2G-mobile",
        "Tulipa2H-bloomer-mobile",
        "Tulipa2H-mobile",
        "Tulipa2I-bloomer-mobile",
        "Tulipa2I-mobile",
        "Tulipa3A-bloomer-mobile",
        "Tulipa3A-mobile",
        "Tulipa3B-bloomer-mobile",
        "Tulipa3B-mobile",
        "Tulipa3C-bloomer-mobile",
        "Tulipa3C-mobile",
        "Tulipa3D-bloomer-mobile",
        "Tulipa3D-mobile",
        "Tulipa3E-bloomer-mobile",
        "Tulipa3E-mobile",
        "Tulipa3F-bloomer-mobile",
        "Tulipa3F-mobile",
        "Tulipa3G-bloomer-mobile",
        "Tulipa3G-mobile",
        "Tulipa3H-bloomer-mobile",
        "Tulipa3H-mobile",
        "Tulipa3I-bloomer-mobile",
        "Tulipa3I-mobile",
        "Viola1A-mobile",
        "Viola1B-mobile",
        "Viola1C-mobile",
        "Viola1D-mobile",
        "Viola1E-mobile",
        "Viola1F-mobile",
        "Viola1G-mobile",
        "Viola1H-mobile",
        "Viola1I-mobile",
        "Viola2A-mobile",
        "Viola2B-mobile",
        "Viola2C-mobile",
        "Viola2D-mobile",
        "Viola2E-mobile",
        "Viola2F-mobile",
        "Viola2G-mobile",
        "Viola2H-mobile",
        "Viola2I-mobile",
        "Viola3A-mobile",
        "Viola3B-mobile",
        "Viola3C-mobile",
        "Viola3D-mobile",
        "Viola3E-mobile",
        "Viola3F-mobile",
        "Viola3G-mobile",
        "Viola3H-mobile",
        "Viola3I-mobile",
        "Artemisia1-mobile",
        "Artemisia2-mobile",
        "Artemisia3-mobile",
        "Artemisia4-mobile",
        "beans",
        "beet",
        "cucumber",
        "eggplant",
        "lettuce",
        "pumpkin",
        "tomatoe_plant",
        "watermelon",
        "wood_end (21)",
        "wood_end",
        "bridge01",
        "bridge02",
        "bridge03",
        "bridge04",
        "bridge_deck",
        "chain_fence",
        "flagstone_path",
        "flagstone_square",
        "Garage",
        "Garden_Hose",
        "Garden_Lawn_Sprinkler",
        "Grass",
        "hangar",
        "log_cut_path",
        "log_cut_square",
        "log_end_path",
        "log_end_square",
        "planck002",
        "plank003",
        "Planter_Table",
        "stone1_reg_alley",
        "stone1_reg_square",
        "stone2_alley",
        "stone2_reg",
        "stone2_reg_alley",
        "stone2_square",
        "FlowerPot1",
        "FlowerPot1grounded",
        "FlowerPot2",
        "FlowerPot2grounded",
        "FlowerPot3",
        "FlowerPot3grounded",
        "Garden_Hoe",
        "Gardening_Gloves",
        "hoe",
        "hose",
        "HosePipeWheel",
        "rake_1",
        "rake_2",
        "round_pot",
        "seed_bag",
        "seed_bag_open",
        "shovel",
        "square_pot",
        "WellingtonBoots",
        "wheelbarrow",
        "rock_01",
        "rock_02",
        "rock_03",
        "rock_04",
        "Stepping_Stones",
        "Stone_A",
        "Stone_A_LP",
        "Stone_B",
        "Stone_B_LP",
        "Stone_C",
        "Stone_C_LP",
        "Stone_D.001",
        "Stone_D",
        "Stone_E.001",
        "Stone_E",
        "StoneA",
        "StoneB",
        "StoneC",
        "StoneD",
        "StoneE",
        "ParticleSystem-Butterfly1",
        "ParticleSystem-Butterfly2",
        "ParticleSystem-FlyingInsect1",
        "ParticleSystem-FlyingInsect2",
        "ParticleSystem-FlyingInsect3",
        "farmer_milk",
        "farmer_show_vegetables",
        "farmer_walk_with_vegetable"
    };

    private UnityMessageManager Manager {
        get {return GetComponent<UnityMessageManager>(); }
        // Manager.SendMessageToFlutter(Convert.ToBase64String(fileData));
    }

    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsConnected) {
            downloadProgress = GameObject.Find("DownloadProgress").GetComponent<Text>();
            var cam = GameObject.Find("ARCamera_" + PhotonNetwork.NickName);
            arCamera = (cam.GetComponent<Camera>() as Camera);
            arSessionOrigin = GameObject.Find("AROrigin_" + PhotonNetwork.NickName);
            arRaycaster = arSessionOrigin.GetComponent<ARRaycastManager>();
            pool = PhotonNetwork.PrefabPool as DefaultPool;

            string assetBundleDirectory = "Assets/AssetBundles";
            String path = Path.Combine(Application.persistentDataPath, assetBundleDirectory) + "/assetbundle_0";
            if(!File.Exists(path)) {
                StartCoroutine(SaveAssetBundleOnDisk());
            } else {
                bundle = AssetBundle.LoadFromFile(path);
                if (bundle == null)
                {
                    downloadProgress.text = "Failed to load AssetBundle!";
                    var task = Task.Run(() => {
                        downloadProgress.gameObject.SetActive(false);
                    });
                    task.Wait(TimeSpan.FromSeconds(3));
                }
                else
                {
                    downloadProgress.text = "Successed to load AssetBundle!";
                    var task = Task.Run(() => {
                        downloadProgress.gameObject.SetActive(false);
                        Init();
                    });
                    task.Wait(TimeSpan.FromSeconds(3));
                }
            }
        }
    }

    IEnumerator SaveAssetBundleOnDisk() {
        string uri = "";
        if(Application.platform == RuntimePlatform.IPhonePlayer) {
            uri = "...";
        } else if(Application.platform == RuntimePlatform.Android) {
            uri = "...";
        } else {
            uri = "...";
        }
        using(UnityWebRequest request = UnityWebRequest.Get(uri)) {
            request.SendWebRequest();
            while(!request.isDone) {
                yield return null;
                downloadProgress.text = "Asset Bundle Download Progress: " + (request.downloadProgress * 100.0f).ToString("N2") + "%";
            }

            string assetBundleDirectory = "Assets/AssetBundles";
            if (!Directory.Exists(Path.Combine(Application.persistentDataPath, assetBundleDirectory)))
            {
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, assetBundleDirectory));
            }

            File.WriteAllBytes(Path.Combine(Application.persistentDataPath, assetBundleDirectory) + "/assetbundle_0", request.downloadHandler.data);
        }

        yield return LoadAssetFromLocalDisk();
    }

    IEnumerator LoadAssetFromLocalDisk() {
        string assetBundleDirectory = "Assets/AssetBundles";
        // 저장한 에셋 번들로부터 에셋 불러오기
        bundle = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath, assetBundleDirectory) + "/assetbundle_0");
        if (bundle == null)
        {
            downloadProgress.text = "Failed to load AssetBundle!";
            var task = Task.Run(() => {
                downloadProgress.gameObject.SetActive(false);
            });
            task.Wait(TimeSpan.FromSeconds(3));
            yield break;
        }
        else
        {
            downloadProgress.text = "Successed to load AssetBundle!";
            var task = Task.Run(() => {
                downloadProgress.gameObject.SetActive(false);
                Init();
            });
            task.Wait(TimeSpan.FromSeconds(3));
        }
    }

    void Init() {
        if(PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient && ES3.KeyExists("length")) {
            for(var i = 0 ; i < ES3.Load<int>("length"); ++i) {
                var obj = PhotonNetwork.Instantiate(Path.GetFileNameWithoutExtension(ES3.Load<string>(numberPad(i) + "_name")), ES3.Load<Vector3>(numberPad(i) + "_position"), ES3.Load<Quaternion>(numberPad(i) + "_rotation"));
                obj.transform.localScale = ES3.Load<Vector3>(numberPad(i) + "_scale");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {   
        //UpdateCenterObject();
        PlaceObjectByTouch();
    }

    String numberPad(int n) {
        return n.ToString().Length >= 3 ? n.ToString() : n.ToString().PadLeft(3, '0');
    }

    public void ChangeRespawnTarget(string args) {
        Int32.TryParse(args, out respawnIndex);
    }

    private void PlaceObjectByTouch() {
        if(Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            if(!isSelect) {
                if(touch.phase == TouchPhase.Began) {
                // 한번만 실행
                    List<ARRaycastHit> hits = new List<ARRaycastHit>();
                    if(arRaycaster.Raycast(touch.position, hits, TrackableType.Planes)) {
                        if(!firstInstantiated) {
                            Manager.SendMessageToFlutter("!first");
                        }
                        Pose hitPose = hits[0].pose;
                        if(bundle != null && pool != null) {
                            GameObject place = bundle.LoadAsset<GameObject>(prefabNames[respawnIndex]);
                            pool.ResourceCache.Add(place.name, place);
                            if(place.GetComponent<MeshRenderer>() == null) {
                                place.AddComponent<MeshRenderer>();
                            }
                            Vector3 correction;
                            if(place.transform.childCount > 0) {
                                Bounds bounds = new Bounds();
                                Renderer[] renderers = place.GetComponentsInChildren<Renderer>();
                                if(renderers.Length > 0) bounds = renderers[0].bounds;
                                foreach(Renderer r in renderers) {
                                    bounds.Encapsulate(r.bounds);
                                }
                                correction = bounds.center;
                            } else {
                                correction = place.transform.GetComponent<Renderer>().bounds.center;
                            }
                            GameObject gardenObj = PhotonNetwork.Instantiate(place.name, new Vector3(hitPose.position.x, place.transform.rotation.x != -90.0f && (correction.y < 0.5f && correction.y > -0.5f) ? hitPose.position.y + correction.y : hitPose.position.y, place.transform.rotation.x == -90.0f && (correction.z < 0.5f && correction.z > -0.5f) ? hitPose.position.z + correction.z : hitPose.position.z), Quaternion.Euler(hitPose.rotation.x + place.transform.eulerAngles.x, hitPose.rotation.y + place.transform.eulerAngles.y, hitPose.rotation.z + place.transform.eulerAngles.z));
                            // reload
                            if(respawnIndex >= 52 && respawnIndex <= 62 && respawnIndex % 2 == 0) {
                                gardenObj.transform.GetComponent<SkinnedMeshRenderer>().material = bundle.LoadAsset<Material>("Cosmos-mobile");
                            } else if(respawnIndex >= 65 && respawnIndex <= 77 && respawnIndex % 2 == 1) {
                                gardenObj.transform.GetComponent<SkinnedMeshRenderer>().material = bundle.LoadAsset<Material>("DaisyHD-mobile");
                            } else if(respawnIndex >= 79 && respawnIndex <= 95 && respawnIndex % 2 == 1) {
                                gardenObj.transform.GetComponent<SkinnedMeshRenderer>().material = bundle.LoadAsset<Material>("Gazania");
                            } else if(respawnIndex >= 110 && respawnIndex <= 116 && respawnIndex % 2 == 0) {
                                gardenObj.transform.GetComponent<SkinnedMeshRenderer>().material = bundle.LoadAsset<Material>("MorningGlory-mobile");
                            } else if(respawnIndex >= 118 && respawnIndex <= 128 && respawnIndex % 2 == 0) {
                                gardenObj.transform.GetComponent<SkinnedMeshRenderer>().material = bundle.LoadAsset<Material>("Narcissus-mobile");
                            } else if(respawnIndex >= 188 && respawnIndex <= 294 && respawnIndex % 2 == 0) {
                                gardenObj.transform.GetComponent<SkinnedMeshRenderer>().material = bundle.LoadAsset<Material>("TulipaHD-mobile");
                            }
                            float height = gardenObj.GetComponent<Renderer>().bounds.size.y;
                            if(height > 1.0f) {
                                Vector3 rescale = gardenObj.transform.localScale;
                                rescale.x = 1.0f * rescale.x / height;
                                rescale.y = 1.0f * rescale.y / height;
                                rescale.z = 1.0f * rescale.z / height;
                                gardenObj.transform.localScale = rescale;
                            }
                            firstInstantiated = true;
                        } else {
                            Debug.Log("Something gone wrong!");
                        }
                    }
                }
            } else {
                if (selectedObj != null && touch.phase == TouchPhase.Moved) {
                    List<ARRaycastHit> hits = new List<ARRaycastHit>();
                    if(arRaycaster.Raycast(touch.position, hits, TrackableType.Planes)) {
                        Pose hitPose = hits[0].pose;
                        selectedObj.transform.position = new Vector3(hitPose.position.x, selectedObj.transform.position.y, hitPose.position.z);
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
            isSelect = true;
            if(hitobj.transform.parent != null) {
                selectedObj = hitobj.transform.parent.gameObject;
            } else {
                selectedObj = hitobj.transform.gameObject;
            }
            
            if(!selectedObj.GetPhotonView().IsMine) {
                var j = 0;
                for(; j < PhotonNetwork.PlayerList.Length && !(PhotonNetwork.PlayerList[j].NickName == PhotonNetwork.NickName); ++j);
                if(j < PhotonNetwork.PlayerList.Length) {
                    selectedObj.GetPhotonView().TransferOwnership(PhotonNetwork.PlayerList[j]);
                }
            }
            curScale = selectedObj.GetPhotonView().transform.localScale;
            yLoc = selectedObj.GetPhotonView().transform.position.y;
            prevAngle = 0.0f;
            Manager.SendMessageToFlutter("!" + selectedObj.name + "selected");
        } else {
            selectedObj = null;
            isSelect = false;
            curScale = new Vector3();
            prevAngle = 0.0f;
            Manager.SendMessageToFlutter("!missing");
        }
    }

    public void Resize(string args) {
        if(selectedObj != null && curScale != null) {
            // selectedObj.transform.localScale = curScale * float.Parse(args);
            selectedObj.GetPhotonView().transform.localScale = curScale * float.Parse(args);
        }
    }

    public void Rotate(string args) {
        
        if(selectedObj != null) {
            // selectedObj.transform.RotateAround(selectedObj.transform.position, selectedObj.transform.forward, yAngle + float.Parse(args));
            // around world y axis
            if(float.Parse(args) - prevAngle > 0) { // increasing
                // selectedObj.GetPhotonView().transform.Rotate(new Vector3(0, 0, 1));
                selectedObj.GetPhotonView().transform.RotateAround(selectedObj.transform.position, Vector3.up, 1.0f);
            } else { //  decreasing
                // selectedObj.GetPhotonView().transform.Rotate(new Vector3(0, 0, -1));
                selectedObj.GetPhotonView().transform.RotateAround(selectedObj.transform.position, Vector3.up, -1.0f);
            }
            prevAngle = float.Parse(args);
        }

    }

    public void AdjustHeight(string args) {
        if(selectedObj != null) {
            // selectedObj.transform.position = new Vector3(selectedObj.transform.position.x, yLoc + float.Parse(args), selectedObj.transform.position.z);
            selectedObj.GetPhotonView().transform.position = new Vector3(selectedObj.GetPhotonView().transform.position.x, yLoc + float.Parse(args), selectedObj.GetPhotonView().transform.position.z);
        }
    }

    public void DestroySelected(string args) {
        if(selectedObj != null) {
            PhotonNetwork.Destroy(selectedObj.GetPhotonView());
        }
    }

    // public void DestroyAll(string args) {
    //     foreach(var obj in GameObject.FindGameObjectsWithTag("GardenObject")) {
    //         PhotonNetwork.Destroy(obj.GetPhotonView());
    //     }
    // }

    public void SaveGardenObjs(string args) {
        var gardenObjs = GameObject.FindGameObjectsWithTag("GardenObject");
        if(ES3.KeyExists("length")) {
            for(var i = 0 ; i < ES3.Load<int>("length"); ++i) {
                ES3.DeleteKey(numberPad(i), "_name");
                ES3.DeleteKey(numberPad(i) + "_position");
                ES3.DeleteKey(numberPad(i) + "_rotation");
                ES3.DeleteKey(numberPad(i) + "_scale");
            }
        }
        if(gardenObjs.Length > 0) {
            ES3.Save("length", gardenObjs.Length);
            for(var i = 0 ; i < gardenObjs.Length; ++i) {
                // prefab name, position, rotation, scale
                ES3.Save<String>(numberPad(i) + "_name", gardenObjs[i].transform.name.Split('(')[0]);
                ES3.Save<Vector3>(numberPad(i) + "_position", gardenObjs[i].transform.position);
                ES3.Save<Quaternion>(numberPad(i) + "_rotation", gardenObjs[i].transform.rotation);
                ES3.Save<Vector3>(numberPad(i) + "_scale", gardenObjs[i].transform.localScale);
            }
        }
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
