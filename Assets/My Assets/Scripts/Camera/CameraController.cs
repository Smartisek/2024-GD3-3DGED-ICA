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
    private Vector3 currentOffset;
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
            currentOffset = offset;
            playerController = player.GetComponent<PlayerController>(); //access player controller script for moving check

        }
    }

    private void LateUpdate()
    {
        if (player == null)
        {
            return;
        }

        // Smoothly follow the player
        Vector3 desiredPosition = player.position + currentOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Handle camera rotation
        HandleCameraRotation();
    }

    private void HandleCameraRotation()
    {
        if (rotationInput != Vector2.zero)
        {
            float horizontalInput = rotationInput.x * rotationSpeed * Time.deltaTime;

            // Rotate the camera around the player
            Quaternion rotation = Quaternion.AngleAxis(horizontalInput, Vector3.up);
            currentOffset = rotation * currentOffset; // Update the offset dynamically

            // Update the camera's position and ensure the correct offset
            transform.position = player.position + currentOffset;

            // Look at the player with offset mathing the initial position
            transform.LookAt(player.position + Vector3.up * 1.7f);
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
