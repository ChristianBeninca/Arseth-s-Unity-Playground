using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Camera _cam;
    RaycastHit hit;

    void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        InputHolder();
    }

    void FixedUpdate()
    {
        #region Debug Interaction
        //if (Physics.Raycast(_cam.transform.position, _cam.transform.TransformDirection(Vector3.forward), out hit, 2f))
        //{
        //    Debug.Log("<color=green>" + hit.collider.gameObject.name + "</color>");
        //    if (hit.collider.gameObject.TryGetComponent(out IInteractable interactable)) 
        //    {
        //        Debug.DrawRay(_cam.transform.position, _cam.transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
        //        //interactable.Interact();
        //    }
        //    else 
        //    {
        //        Debug.DrawRay(_cam.transform.position, _cam.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
        //    }
        //}
        //else
        //{
        //    Debug.DrawRay(_cam.transform.position, _cam.transform.TransformDirection(Vector3.forward) * 2f, Color.yellow);
        //}
        #endregion
    }

    void InputHolder()
    {
        if (Input.GetKeyDown(KeyCode.E)) Interact();
    }

    void Interact()
    {
        if (!Physics.Raycast(_cam.transform.position, _cam.transform.TransformDirection(Vector3.forward), out hit, 2f)) return;

        if (hit.collider.gameObject.TryGetComponent(out IInteractable interactable))
        {
            interactable.Interact();
        }
        else
        {
            Debug.Log("<color=yellow>Not an interactable!</color>");
        }
    }
}