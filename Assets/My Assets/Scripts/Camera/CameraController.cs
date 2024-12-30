using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    #region Variables
    [Header("Camera Settings")]
    [Tooltip("Drag target to follow")]
    [SerializeField] Transform player;
    [Tooltip("Smoothness of camera movement")]
    [SerializeField] float smoothSpeed = 8f;
    [Tooltip("Offset from target")]
    [SerializeField] Vector3 offset;
    [Tooltip("Camera rotation speed")]
    [SerializeField] float rotationSpeed = 5f;

    private PlayerInput playerInput;
    private Vector2 rotationInput;
    private PlayerController playerController;
    #endregion

    #region Subscription Methods
    //Subscribe methods 
    private void OnEnable()
    {
        playerInput.Main.Enable(); //enable the main action map
        playerInput.Main.Rotation.performed += OnRotate; //subscribe to the rotation performed event
        playerInput.Main.Rotation.canceled  += OnRotateCancel; //subscribe to the rotation canceled event
    }

    private void OnDisable()
    {
        playerInput.Main.Disable();
        playerInput.Main.Rotation.performed -= OnRotate;
        playerInput.Main.Rotation.canceled -= OnRotateCancel;
    }
    #endregion

    private void Awake()
    {
        playerInput = new PlayerInput(); //access the PlayerInput class
    }


    private void Start()
    {
        if (player != null) //make sure player is dragged in the inspector 
        {
        
            offset = transform.position - player.position; //get offset for the right camera positon
            playerController = player.GetComponent<PlayerController>(); //access player controller script for moving check

        }
    }

    private void Update()
    {
        if (player == null) { return; } //if player is not dragged in the inspector, return

        HandleCameraRotation(); //call the camera rotation method

        //Logic for following the player with the camera
        Vector3 desiredPosition = player.position + offset; //get the desired position of the camera 
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime); //make the movement smoother
        transform.position = smoothedPosition; //update the camera position
    }

    private void HandleCameraRotation()
    {
        if (rotationInput != Vector2.zero && playerController != null && !playerController.IsMoving) //check if player is moving and if the rotation input is not zero
        {
            float horizontalInput = rotationInput.x * rotationSpeed; //get the horizontal input, we dont need vertical

            // Rotate the camera around the player
            transform.RotateAround(player.position, Vector3.up, horizontalInput);

            // Update the offset to maintain the same distance from the player
            offset = transform.position - player.position;
        }
    }

    #region Callback Context Methods for Input System
    private void OnRotate(InputAction.CallbackContext context)
    {
        rotationInput = context.ReadValue<Vector2>();
    }

    private void OnRotateCancel(InputAction.CallbackContext context)
    {
        rotationInput = Vector2.zero;
    }
    #endregion
}
