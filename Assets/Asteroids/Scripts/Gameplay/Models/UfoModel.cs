using UnityEngine;

public class UfoModel : IEnemy, IObjectPoolObject
{
    public bool IsActive { get; private set; }
    public Vector3 Position { get; private set; }
    public EnemyType Type => EnemyType.Ufo;
    public int Health { get; private set; }
    private Vector3 _velocity;
    private readonly float _speed;

    public UfoModel()
    {
        var config = ConfigLoader.LoadEnemyConfig();
        _speed = config.ufo.speed;
        IsActive = false;
        Health = 2;
        Debug.Log($"UfoModel: Initialized with speed={_speed}, health={Health}");
    }

    public void Activate(Vector3 position, Vector3 direction, float size = 1f, bool isFragment = false)
    {
        Position = position;
        _velocity = direction.normalized * _speed;
        IsActive = true;
        Debug.Log($"UfoModel: Activated at {position} with velocity {_velocity}");
    }

    public void Update(float deltaTime)
    {
        if (!IsActive) return;
        Position += _velocity * deltaTime;
        Debug.Log($"UfoModel: Updated to {Position}");
    }

    public void Update()
    {
        Update(Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Deactivate();
        }
        Debug.Log($"UfoModel: Took {damage} damage, health={Health}, isActive={IsActive}");
    }

    public void Deactivate()
    {
        IsActive = false;
        Debug.Log($"UfoModel: Deactivated at {Position}");
    }

    public void OnReset()
    {
        IsActive = false;
        Position = Vector3.zero;
        _velocity = Vector3.zero;
        Health = 2;
        Debug.Log("UfoModel: Reset");
    }
}