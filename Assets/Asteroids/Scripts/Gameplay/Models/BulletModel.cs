using UnityEngine;

public class BulletModel
{
    public Vector3 Position { get; private set; }
    public Vector3 Velocity { get; private set; }
    public bool IsActive { get; private set; }
    private float _speed;

    public BulletModel(Vector3 position, Quaternion rotation)
    {
        Position = position;
        _speed = ConfigLoader.LoadPlayerConfig().bulletSpeed;
        Velocity = rotation * Vector3.forward * _speed;
        IsActive = true;
    }

    public void Reset(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Velocity = rotation * Vector3.forward * _speed;
        IsActive = true;
    }

    public void PositionUpdate()
    {
        if (!IsActive) return;
        
        Position += Velocity * Time.deltaTime;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}