using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [Tooltip("Drag target to follow")]
    [SerializeField] Transform player;
    [Tooltip("Smoothness of camera movement")]
    [SerializeField] float smoothSpeed = 8f;
    [Tooltip("Offset from target")]
    [SerializeField] Vector3 offset;

    private void Start()
    {
        if (player != null)
        {
            offset = transform.position - player.position;
        }
    }

    private void Update()
    {
       if(player == null) { return; }

       Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed*Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
