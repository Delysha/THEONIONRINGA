using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationScript : MonoBehaviour

{
    public float mouseSensitivity = 2.0f;    // Sensitivity for mouse movement
    public float verticalLookLimit = 80f;    // Limit for looking up/down

    private float verticalRotation = 0f;     // Tracks vertical rotation

    void Start()
    {
        // Lock and hide the cursor for FPS-like control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        RotateCamera();
        HandleClick();
    }

    void RotateCamera()
    {
        // Mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Horizontal rotation (Y-axis)
        transform.Rotate(0, mouseX, 0);

        // Vertical rotation (X-axis) with clamping
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);

        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    void HandleClick()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Clicked on: " + hit.collider.name);

                // Check if the object has an interactable script
                if (hit.collider.TryGetComponent(out IClickable clickable))
                {
                    clickable.OnClick();
                }
            }
        }
    }
}
