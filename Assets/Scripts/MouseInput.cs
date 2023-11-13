using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    public Camera cam;
    public Transform player;
    public float mouseSensivity;
    float xRotation;
    float fieldOfViewSpeed = 24f;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;   
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        float mouseXPos = Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;
        float mouseYPos = Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;

        xRotation -= mouseYPos;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        player.Rotate(Vector3.up * mouseXPos);

        CamFieldOfView();
    }

    private void CamFieldOfView() // Koþarken-yürürken kamera görüþ açýsýný geniþletip daraltmaya yarar.
    {
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - (Input.GetKey(KeyCode.LeftShift) ? -1 : 1) * fieldOfViewSpeed * Time.deltaTime, 66f, 72f);
    }

}
