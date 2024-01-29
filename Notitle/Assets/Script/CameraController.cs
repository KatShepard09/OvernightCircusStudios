using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private CinemachineTransposer cinemachineTransposer;
    private Vector3 targetFollowOffset;

    private void Start()
    {
       cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }
    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }   
   
    private void HandleMovement()
    {
        Vector3 inputMoveDir = new Vector3(0, 0, 0);//handles camera movment 
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z = +1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = +1f;
        }

        float moveSpeed = 10f;

        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;//handles movement speed.
    }

    private void HandleRotation()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);//handles camera rotaion
        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y = +1f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y = -1f;
        }

        float rotationSpeed = 100F;
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;//handles camera roation speed
    }

    private void HandleZoom()
    {

        float zoomamount = 1f;
        if (Input.mouseScrollDelta.y > 0)//Controls mouse zoom in and out
        {
            targetFollowOffset.y -= zoomamount;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetFollowOffset.y += zoomamount;
        }

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        float zoomSpeed = 5f;
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);//zoom speed
    }
}
