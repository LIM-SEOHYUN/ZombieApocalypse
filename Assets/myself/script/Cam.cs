using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public float mouseSensitivity = 400f;

    private float MouseY;
    private float MouseX;

    void FixedUpdate()
    {
        Rotate();
    }
    private void Rotate()
    {

        MouseX += Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        MouseY -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;
        MouseY = Mathf.Clamp(MouseY, -90f, 90f);
        transform.localRotation = Quaternion.Euler(MouseY, MouseX, 0f);
    }
}
