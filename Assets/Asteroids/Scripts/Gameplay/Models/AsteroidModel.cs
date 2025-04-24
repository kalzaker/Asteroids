using UnityEngine;

public class AsteroidModel : IEnemy, IObjectPoolObject
{
    public Vector3 Position { get; private set; }
    public Vector3 Velocity { get; private set; }
    public float Size { get; private set; }
    public bool IsActive { get; private set; }
    public EnemyType Type => EnemyType.Asteroid;
    public int Health { get; private set; }
    public bool IsFragment { get; private set; }
    private readonly float _speed;

    public AsteroidModel()
    {
        var config = ConfigLoader.LoadEnemyConfig();
        _speed = Random.Range(config.asteroid.minSpeed, config.asteroid.maxSpeed);
        IsActive = false;
        Debug.Log($"AsteroidModel: Initialized with speed={_speed}");
    }

    public void Activate(Vector3 position, Vector3 direction, float size = 1f, bool isFragment = false)
    {
        Position = position;
        Velocity = direction.normalized * _speed;
        Size = size;
        IsActive = true;
        Health = isFragment ? 1 : ConfigLoader.LoadEnemyConfig().asteroid.health;
        IsFragment = isFragment;
        Debug.Log($"AsteroidModel: Activated at {position}, velocity={Velocity}, size={size}, health={Health}, isFragment={isFragment}");
    }

    public void Update(float deltaTime)
    {
        if (!IsActive) return;
        Position += Velocity * deltaTime;
        Debug.Log($"AsteroidModel: Updated to {Position}");
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
        Debug.Log($"AsteroidModel: Took {damage} damage, health={Health}, isActive={IsActive}");
    }

    public void Deactivate()
    {
        IsActive = false;
        Debug.Log($"AsteroidModel: Deactivated at {Position}");
    }

    public void OnReset()
    {
        IsActive = false;
        Position = Vector3.zero;
        Velocity = Vector3.zero;
        Size = ConfigLoader.LoadEnemyConfig().asteroid.size;
        Health = ConfigLoader.LoadEnemyConfig().asteroid.health;
        IsFragment = false;
        Debug.Log("AsteroidModel: Reset");
    }
}