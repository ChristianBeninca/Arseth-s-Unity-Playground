using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Portal : MonoBehaviour
{
    bool portalLoaded = false;
    public Transform playerCamera, otherPortal;
    Camera otherCamera;
    Material thisMaterial;
    [SerializeField] string inputKey, outputKey;
        public string InputKey { get => inputKey; }
        public string OutputKey { get => outputKey; set => outputKey = value; }
    [SerializeField] Shader portalShader;

    void Start()
    {
        DontDestroyOnLoad(this);
        Setup();
    }

    void Setup()
    {
        playerCamera = Camera.main.transform;
        thisMaterial = GetComponentInChildren<Renderer>().material = new Material(portalShader);

        int sceneId = PortalLinks.Instance.GetSceneByKey(outputKey);
        Debug.Log("sceneid: " + sceneId);
        if (!SceneManager.GetSceneByBuildIndex(sceneId).isLoaded)
        {
            var progress = SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Additive);
            progress.completed += (op) =>
            {
                otherPortal = PortalLinks.Instance.GetPortalByKey(outputKey).transform;
                otherCamera = otherPortal.GetComponentInChildren<Camera>();
                PortalTextureSetup();
            }; //Tentar entender depois porque eu preciso passar "op" e n�o posso deixar os par�nteses vazios.
        }
        else
        {
            otherPortal = PortalLinks.Instance.GetPortalByKey(outputKey).transform;
            otherCamera = otherPortal.GetComponentInChildren<Camera>();
            PortalTextureSetup();
        }
        portalLoaded = true;
    }

    void PortalTextureSetup()
    {
        otherCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        thisMaterial.mainTexture = otherCamera.targetTexture;
    }

    void SyncCameraTransform()
    {
        Vector3 playerOffsetFromPortal = playerCamera.position - transform.position;
        otherCamera.transform.position = otherPortal.position + playerOffsetFromPortal;

        float angularDifferenceBetweenPortalRotations = Quaternion.Angle(transform.rotation, otherPortal.rotation);
        Quaternion portalRotationDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations + 180, Vector3.up);
        Vector3 newCameraDirection = portalRotationDifference * playerCamera.forward;
        otherCamera.transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
    }

    void Update()
    {
        if(portalLoaded)
            SyncCameraTransform();
    }

    Transform GetOtherPortalByKey(string outputKey)
    {
        Debug.Log("<color=red>Feature not implemented</color>");
        GameObject output = GameObject.FindGameObjectWithTag("");
        return output.transform;
    }
}
