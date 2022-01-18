using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] [Range(100,400)] float mouseSensitivity;
    [SerializeField] Animator anim;

    float 
        playerSpeed = 3f,
        StandHeight = 1.5f,
        DuckHeight = .75f,
        sprintSpeed = 1.5f,
        walkSpeed = 0.5f,
        jumpHeight = 1f,
        mouseX,
        mouseY,
        xRotation,
        gravityValue = -9.81f * 1.5f;
    bool 
        sprint,
        walk,
        duck,
        isGrounded;  

    Transform 
        playerCamera;
    CharacterController
        ct;
    CombatManager
        combat;

    Vector3
        fallVelocity;
    LayerMask
        groundMasks;


    void Awake()
    {
        groundMasks = LayerMask.GetMask("Ground", "Static");
        playerCamera = Camera.main.transform.parent;
        ct = gameObject.AddComponent<CharacterController>();
        ct.radius = .35f;
        ct.height = StandHeight;
        ToggleCursor(1);
        combat = new CombatManager(100, 10, anim);
    }

    private void Start()
    {
    }

    void Update()
    {
        InputHolder();
        MouseLook();
        MoveController();
        Gravity();
    }

    void MouseLook()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void MoveController()
    {
        Vector3 movement = (transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical"));
        if (movement != Vector3.zero)
        {
            if (sprint) movement *= (playerSpeed * sprintSpeed);
            else if (walk) movement *= (playerSpeed * walkSpeed);
            else movement *= playerSpeed;
         
            ct.Move(movement * Time.deltaTime);
        }
    }

    void Jump()
    {
        if (isGrounded)
        fallVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
    }

    void Gravity()
    {
        GroundCheck();

        if (!isGrounded) fallVelocity += Vector3.up * gravityValue * Time.deltaTime;
            
        ct.Move(fallVelocity * Time.deltaTime);
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.position + (Vector3.down * ct.height / 2), .3f, groundMasks);
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * ct.height / 2), Color.magenta);
        if (isGrounded && fallVelocity.y < 0)
            fallVelocity.y = -2f;
    }

    void InputHolder()
    {
        // Just on Editor
        if (Input.GetKeyDown(KeyCode.X)) { ToggleCursor(); }

        // Duck
        if (Input.GetKeyDown(KeyCode.LeftControl)) { ToggleDuck(); }

        // Walk and Sprint
        if (Input.GetKeyDown(KeyCode.LeftShift) && !walk && Input.GetKey(KeyCode.W)) { sprint = true; }
        if (Input.GetKeyDown(KeyCode.LeftAlt) && !sprint) { walk = true; }
        
        if (Input.GetKeyUp(KeyCode.LeftShift)) { sprint = false; }
        if (Input.GetKeyUp(KeyCode.LeftAlt)) { walk = false; }

        // Jump
        if (Input.GetButtonDown("Jump")) { Jump(); }

        //CombatManager.cs
        if (Input.GetKeyDown(KeyCode.Mouse0)){ combat.Attack(); }
    }

    void ToggleCursor(int mode = -1)
    {
        switch (mode)
        {
            case 0:
                Cursor.lockState = CursorLockMode.None;
                break;

            case 1:
                Cursor.lockState = CursorLockMode.Locked;
                break;

            default:
                if (Cursor.lockState == CursorLockMode.Locked)
                    Cursor.lockState = CursorLockMode.None;
                else
                    Cursor.lockState = CursorLockMode.Locked;
                break;
        }
    }

    void ToggleDuck(int mode = -1)
    {
        StopCoroutine(ToggleDuckRoutine());
        switch (mode)
        {
            case 0:
                duck = false;
                break;
            case 1:
                duck = true;
                break;
            default:
                duck = !duck;
                break;
        }

        StartCoroutine(ToggleDuckRoutine());
    }

    IEnumerator ToggleDuckRoutine()
    {
        float lerpTime = .08f;
        float enumFPS =  EnumFPS(60);
        

        while (ct.height > DuckHeight && duck)
        {
            ct.height = Mathf.Lerp(ct.height, DuckHeight, lerpTime);
            yield return new WaitForSeconds(enumFPS);
        }

        while (ct.height < StandHeight && !duck)
        {
            ct.height = Mathf.Lerp(ct.height, StandHeight, lerpTime);
            yield return new WaitForSeconds(enumFPS);
        }

        yield return null;
    }

    float EnumFPS(int fps) // Returns Expected framerate conerted to float
    {
        return (1 / fps);

    }
}
