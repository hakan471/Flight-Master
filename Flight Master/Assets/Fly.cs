using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    public Joystick joystick;
    private float flyspeed = 5;
    private float yavamount = 120;
    private float yav;
    private void Update()
    {
        transform.position += transform.forward * flyspeed * Time.deltaTime;

        float horizontalInput = joystick.Horizontal;
        float verticalInput = joystick.Vertical;

        yav += horizontalInput * yavamount * Time.deltaTime;

        float pitch = Mathf.Lerp(0, 20, Mathf.Abs(verticalInput)) * Mathf.Sign(verticalInput);
        float roll = Mathf.Lerp(0, 30, Mathf.Abs(horizontalInput)) * -Mathf.Sign(horizontalInput);

        transform.localRotation = Quaternion.Euler(Vector3.up * yav + Vector3.right * pitch + Vector3.forward * roll);
    }
}
