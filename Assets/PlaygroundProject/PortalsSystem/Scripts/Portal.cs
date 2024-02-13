using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private bool portalLoaded = false;
    private bool playerIsOverlapping = false;
    private Transform playerCamera;
    private Transform otherPortal;
    private Camera otherCamera;
    private Material thisMaterial;
    private GameObject player;

    [SerializeField] string inputKey;
    [SerializeField] string outputKey;

    [SerializeField] Shader portalShader;
    [SerializeField] BoxCollider portalCollider;

    #region Properties
    public string InputKey 
    {
        get => inputKey;
    }
    
    public string OutputKey 
    {
        get => outputKey; 
        set => outputKey = value;
    }
    #endregion

    void Start()
    {
        DontDestroyOnLoad(this);
        Setup();
    }

    void Update()
    {
        if (portalLoaded)
            SyncCameraTransform();

        if (playerIsOverlapping)
        {
            Teleporter();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            #region Código antigo pra substituir o sistema do brackeys
            //Debug.Log("<color=green>chegou</color>");

            //player.transform.position = new Vector3(0, 5, 0);


            //float rotationDiff = -Quaternion.Angle(transform.rotation, otherPortal.rotation);
            //rotationDiff += 180;
            //player.transform.Rotate(Vector3.up, rotationDiff);

            //Vector3 portalToPlayer = player.transform.position - transform.position;
            //Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
            //player.transform.position = otherPortal.position + positionOffset;
            #endregion

            playerIsOverlapping = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsOverlapping = false;
        }
    }

    void Teleporter()
    {
        Vector3 portalToPlayer = player.transform.position - (transform.position + portalCollider.center);
        float dotProduct = Vector3.Dot(transform.forward, portalToPlayer);

        if (dotProduct > 0f) //Teleport
        {
            float rotationDiff = -Quaternion.Angle(transform.rotation, otherPortal.rotation);
            rotationDiff += 180;

            player.transform.Rotate(Vector3.up, rotationDiff);
            player.transform.position = otherPortal.position + portalToPlayer;
        }
    }

    void Setup()
    {
        playerCamera = Camera.main.transform;
        player = GameObject.FindGameObjectWithTag("Player");
        thisMaterial = GetComponentInChildren<Renderer>().material = new Material(portalShader);
        portalCollider = GetComponent<BoxCollider>();

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

    Transform GetOtherPortalByKey(string outputKey)
    {
        Debug.Log("<color=red>Feature not implemented</color>");
        GameObject output = GameObject.FindGameObjectWithTag("");
        return output.transform;
    }
}
