using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    Transform playerCamera;
    [SerializeField] Transform otherPortal; // Fazer um get pra essa variável de forma dinâmica
    Camera otherCamera;
    Material thisMaterial;
    [SerializeField] string inputKey, outputKey;
    [SerializeField] Shader portalShader;

    void Start()
    {
        Setup();
        PortalIDSetup();
        PortalTextureSetup();
    }

    void Update()
    {
        SyncCameraTransform();
    }

    void Setup()
    {
        playerCamera = Camera.main.transform;
        thisMaterial = GetComponentInChildren<Renderer>().material = new Material(portalShader);

        // otherPortal = GetOtherPortalByKey(outputId);
        otherCamera = otherPortal.GetComponentInChildren<Camera>();
    }

    void PortalIDSetup()
    {
        PortalManager.AddPortalToList(inputKey, gameObject, gameObject.scene);
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

    Transform GetOtherPortalByKey(string outputKey)
    {
        Debug.Log("<color=red>Feature not implemented</color>");
        GameObject output = GameObject.FindGameObjectWithTag("");
        return output.transform;
    }
}
