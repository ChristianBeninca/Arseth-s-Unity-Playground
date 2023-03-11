using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    [SerializeField] [Range(100,400)] float mouseSensitivity;
    [SerializeField] Animator anim;

    float playerSpeed = 3f;
    float StandHeight = 1.5f;
    float DuckHeight = .75f;
    float sprintSpeed = 1.5f;
    float walkSpeed = 0.5f;
    float jumpHeight = 1f;
    float mouseX;
    float mouseY;
    float xRotation;
    float gravityValue = -9.81f * 1.5f;
    bool sprint;
    bool walk;
    bool duck;
    bool isGrounded;  

    Transform playerCamera;
    CharacterController characterController;

    Vector3 fallVelocity;
    LayerMask groundMasks;


    void Awake()
    {
        groundMasks = LayerMask.GetMask("Ground", "Static");
        playerCamera = Camera.main.transform.parent;
        characterController = gameObject.AddComponent<CharacterController>();
        characterController.radius = .35f;
        characterController.height = StandHeight;
    }

    private void Start()
    {
    }

    void Update()
    {
        InputHolder();
        MouseLook();
        Move();
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

    void Move()
    {
        Vector3 movement = (transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical"));
        if (movement != Vector3.zero)
        {
            if (sprint) movement *= (playerSpeed * sprintSpeed);
            else if (walk) movement *= (playerSpeed * walkSpeed);
            else movement *= playerSpeed;
         
            characterController.Move(movement * Time.deltaTime);
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
            
        characterController.Move(fallVelocity * Time.deltaTime);
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.position + (Vector3.down * characterController.height / 2), .3f, groundMasks);
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * characterController.height / 2), Color.magenta);
        if (isGrounded && fallVelocity.y < 0)
            fallVelocity.y = -2f;
    }

    void InputHolder()
    {
        // Duck
        if (Input.GetKeyDown(KeyCode.LeftControl)) { ToggleDuck(); }

        // Walk and Sprint
        if (Input.GetKeyDown(KeyCode.LeftShift) && !walk && Input.GetKey(KeyCode.W)) { sprint = true; }
        if (Input.GetKeyDown(KeyCode.LeftAlt) && !sprint) { walk = true; }
        
        if (Input.GetKeyUp(KeyCode.LeftShift)) { sprint = false; }
        if (Input.GetKeyUp(KeyCode.LeftAlt)) { walk = false; }

        // Jump
        if (Input.GetButtonDown("Jump")) { Jump(); }
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
        

        while (characterController.height > DuckHeight && duck)
        {
            characterController.height = Mathf.Lerp(characterController.height, DuckHeight, lerpTime);
            yield return new WaitForSeconds(enumFPS);
        }

        while (characterController.height < StandHeight && !duck)
        {
            characterController.height = Mathf.Lerp(characterController.height, StandHeight, lerpTime);
            yield return new WaitForSeconds(enumFPS);
        }

        yield return null;
    }

    float EnumFPS(int fps) // Returns Expected framerate converted to float
    {
        return (1 / fps);

    }
}
