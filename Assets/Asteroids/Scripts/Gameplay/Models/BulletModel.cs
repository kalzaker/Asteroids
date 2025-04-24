using UnityEngine;

public class BulletModel : IPoolObject, IObjectPoolObject
{
    public Vector3 Position { get; private set; }
    public bool IsActive { get; private set; }
    public float Lifetime { get; private set; }
    private Vector3 _velocity;
    private readonly float _speed;
    private readonly float _maxLifetime;

    public BulletModel()
    {
        var config = ConfigLoader.LoadBulletConfig();
        _speed = config.speed;
        _maxLifetime = config.maxLifetime;
        IsActive = false;
        Debug.Log($"BulletModel: Initialized with speed={_speed}, maxLifetime={_maxLifetime}");
    }

    public void Activate(Vector3 position, Vector3 direction)
    {
        Position = position;
        _velocity = direction.normalized * _speed;
        Lifetime = 0f;
        IsActive = true;
        Debug.Log($"BulletModel: Activated at {position} with velocity {_velocity}");
    }

    public void Update()
    {
        if (!IsActive) return;
        Position += _velocity * Time.deltaTime;
        Lifetime += Time.deltaTime;
        if (Lifetime >= _maxLifetime)
        {
            Deactivate();
        }
        Debug.Log($"BulletModel: Updated to {Position}, isActive={IsActive}");
    }

    public void Deactivate()
    {
        IsActive = false;
        Debug.Log($"BulletModel: Deactivated at {Position}");
    }

    public void OnReset()
    {
        IsActive = false;
        Position = Vector3.zero;
        _velocity = Vector3.zero;
        Lifetime = 0f;
        Debug.Log("BulletModel: Reset");
    }
}