using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class cameraMovement : MonoBehaviour
{

    [SerializeField] public bool useEdgeScrolling = false;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private float OrthographicSizeMax = 5.8f;
    [SerializeField] private float OrthographicSizeMin = 3.8f;

    private Vector3 moveDir = new Vector3(0, 0, 0);
    private float moveSpeed = 25f;

    private float targetOrthographicSize = 5.8f;

    private float xMin = -12f;
    private float xMax = 12f;

    public void ToggleEdgeScroll()
    {
        useEdgeScrolling = !useEdgeScrolling;
    }

    private void FixedUpdate()
    {
        HandleCameraZoom();
        EdgeScroll();
        CheckInput();
        ApplyMovement();

        moveDir = Vector3.zero;

        if (moveDir != Vector3.zero)
        {
            ApplyMovement();
        }

    }

    private void CheckInput()
    {
        if (Input.GetKey(KeyCode.A) && transform.position.x > xMin)
        {
            moveDir.x = -1f;
        }

        if (Input.GetKey(KeyCode.D) && transform.position.x < xMax)
        {
            moveDir.x = +1f;
        }
    }

    private void ApplyMovement()
    {
        var pos = transform.position;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        pos.x = Mathf.Clamp(transform.position.x, -10.0f, 10.0f);
        transform.position = pos;
    }

    private void HandleCameraZoom()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            targetOrthographicSize -= 0.5f;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetOrthographicSize += 0.5f;
        }

        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, OrthographicSizeMin, OrthographicSizeMax);

        float zoomSpeed = 4f;
        
        cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.OrthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);
    }

    private void EdgeScroll()
    {
        if (useEdgeScrolling)
        {
            int edgeScrollSize = 100;

            if (Input.mousePosition.x < edgeScrollSize)
            {
                moveDir.x = -1f;
            }

            if (Input.mousePosition.x > Screen.width - edgeScrollSize)
            {
                moveDir.x = +1f;
            }
        }
    }
}
