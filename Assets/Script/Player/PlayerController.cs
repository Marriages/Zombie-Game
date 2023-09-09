using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("EnemyWeapon"))
        {
            Debug.Log("Hit!");
        }
    }
    /*
    PlayerView playerView;
    PlayerModel playerModel;
    InputController inputController;
    CharacterController characterController;
    Vector3 inputDir;
    public float moveSpeed = 5f;
    float gravityValue = 9.8f;
    Vector3 gravityVector;

    float rotationSpeed = 3f;
    Vector2 moveInput;
    Vector2 lookInput;
    float maxVerticalAngle = 45f;



    private void Awake()
    {
        inputController = new InputController();
    }
    private void Start()
    {
        gravityVector = Vector3.down * gravityValue * Time.fixedDeltaTime;      //중력 설정을 위한 중력값 설정.
    }

    private void OnEnable()
    {
        InputSetting();
        FindComponent();
    }
    private void OnDisable()
    {
        InputDisSetting();
    }
    void FindComponent()
    {
        playerView = GetComponent<PlayerView>();
        characterController=GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        PlayerLook();
        //rigid.MovePosition(rigid.position + moveSpeed * Time.fixedDeltaTime * inputDir);
        characterController.Move(transform.TransformDirection(inputDir) * moveSpeed * Time.fixedDeltaTime);
            //moveSpeed * Time.fixedDeltaTime * inputDir);
        if (!characterController.isGrounded)
        {
            characterController.Move(gravityVector);
        }
        
    }

    void InputSetting()
    {
        inputController.Player.Enable();
        inputController.Player.Move.performed += PlayerMove;
        inputController.Player.Move.canceled += PlayerMove;
        inputController.Player.Crouch.performed += PlayerCrouch;
        inputController.Player.Run.performed += PlayerRun;
        inputController.Player.Run.canceled += PlayerRun;
        inputController.Player.Attack.performed += PlayerAttack;
    }


    void InputDisSetting()
    {
        inputController.Player.Attack.performed -= PlayerAttack;
        inputController.Player.Run.canceled -= PlayerRun;
        inputController.Player.Run.performed -= PlayerRun;
        inputController.Player.Crouch.performed -= PlayerCrouch;
        inputController.Player.Move.canceled -= PlayerMove;
        inputController.Player.Move.performed -= PlayerMove;
        inputController.Player.Disable();
    }

    void PlayerLook()
    {
        lookInput = inputController.Player.Look.ReadValue<Vector2>();
        float horizontalLook = lookInput.x * rotationSpeed * Time.fixedDeltaTime;
        float verticalLook = lookInput.y * rotationSpeed * Time.fixedDeltaTime;

        transform.Rotate(Vector3.up * horizontalLook);
        float currentXRotation = Camera.main.transform.eulerAngles.x;
        float newVerticalRotation = currentXRotation - verticalLook;
        newVerticalRotation = Mathf.Clamp(newVerticalRotation, -maxVerticalAngle, maxVerticalAngle); // 상하 회전 제한
        Camera.main.transform.rotation = Quaternion.Euler(newVerticalRotation, transform.eulerAngles.y, 0f); // 카메라의 상하 회전 설정
        //Camera.main.transform.Rotate(Vector3.left *  verticalLook);
    }
    private void PlayerAttack(InputAction.CallbackContext obj)
    {
        Debug.Log("PlayerAttack");
        playerView.PlayerAttack();
    }

    private void PlayerRun(InputAction.CallbackContext obj)
    {
        //Debug.Log(obj);
        playerView.PlayerRunning(obj.performed);
            
    }

    private void PlayerCrouch(InputAction.CallbackContext obj)
    {
        
    }

    private void PlayerMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector3>().normalized;
        if (inputDir == Vector3.zero)
            playerView.PlayerMoving(false);
        else
            playerView.PlayerMoving(true);

    }*/
}
