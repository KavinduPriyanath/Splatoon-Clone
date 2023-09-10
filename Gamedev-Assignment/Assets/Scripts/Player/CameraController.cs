using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;
    
    [SerializeField]
    private Transform _playerBody;
    private float xRotation = 0f;

    public float minAngle;
    public float maxAngle;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        minAngle = -90f;
        maxAngle = 90f;
    }

    private void Update()
    {

        CameraRotation();
        
    }

    public void CameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minAngle, maxAngle);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        _playerBody.Rotate(Vector3.up * mouseX);
    }
}
