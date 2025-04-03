using UnityEngine;

public class EnemyFactory
{
    private readonly ShipModel _playerShip;
    private float _spawnDistance = 600f;
    private float _speed = 5f;
    private float _asteroidSize = 3f;

    public EnemyFactory(ShipModel playerShip)
    {
        _playerShip = playerShip;
    }

    public IEnemy Create(EnemyType type)
    {
        Vector3 spawnPosition = Random.onUnitSphere * _spawnDistance;

        switch (type)
        {
            case EnemyType.Asteroid:
                Vector3 direction = Random.insideUnitSphere.normalized;
                return new AsteroidModel(spawnPosition, direction, _speed, _asteroidSize);
            case EnemyType.Ufo:
                return new UfoModel(spawnPosition, _playerShip);
            default:
                throw new System.ArgumentException($"Unknown enemy type: {type}");
        }
    }
}
