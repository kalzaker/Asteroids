using UnityEngine;

public class AsteroidModel : IEnemy
{
    public Vector3 Position { get; private set; }
    public Vector3 Velocity { get; private set; }
    public float Size { get; private set; }
    public bool IsActive { get; private set; }
    public EnemyType Type => EnemyType.Asteroid;
    public int Health { get; private set; }
    public bool IsFragment { get; private set; }

    private readonly EnemyPool _enemyPool;

    public AsteroidModel(Vector3 spawnPosition, Vector3 direction, float speed, float size, EnemyPool enemyPool, bool isFragment = false)
    {
        Position = spawnPosition;
        Velocity = direction.normalized * speed;
        Size = size;
        IsActive = true;
        _enemyPool = enemyPool;
        Health = isFragment ? 1 : ConfigLoader.LoadEnemyConfig().asteroid.health;
        IsFragment = isFragment;
    }

    public void PositionUpdate()
    {
        if (!IsActive) return;
        
        Position += Velocity * Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            if (!IsFragment)
            {
                _enemyPool.SpawnFragments(Position);
            }
            Deactivate();
        }
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
