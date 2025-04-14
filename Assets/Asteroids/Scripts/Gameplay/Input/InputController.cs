using UnityEngine;

public class InputController
{
    public float GetThrustInput()
    {
        float forward = Input.GetKey(KeyCode.W) ? 1 : 0;
        float backward = Input.GetKey(KeyCode.S) ? -1 : 0;
        return forward + backward;
    }

    public Vector3 GetRotationInput()
    {
        float pitch = Input.GetAxisRaw("Mouse Y");
        float yaw = Input.GetAxisRaw("Mouse X");
        float roll = Input.GetAxisRaw("Horizontal");
        return new Vector3(-pitch, yaw, -roll);
    }

    public bool IsShooting()
    {
        return Input.GetMouseButton(0);
    }

    public bool IsShootingLaser()
    {
        return Input.GetMouseButtonDown(1);
    }
}