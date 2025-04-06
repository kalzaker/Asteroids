using UnityEngine;

public class EnemyFactory
{
    private readonly ShipModel _playerShip;
    private readonly EnemyConfig _enemyConfig;
    private readonly WorldConfig _worldConfig;
    private float _spawnAngle = 45f;

    public EnemyFactory(ShipModel playerShip)
    {
        _playerShip = playerShip;
        _enemyConfig = ConfigLoader.LoadEnemyConfig();
        _worldConfig = ConfigLoader.LoadWorldConfig();
    }

    public IEnemy Create(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Asteroid:
                Vector3 spawnPosition = Random.onUnitSphere * _worldConfig.worldSize;
                Vector3 baseDirection = (_playerShip.Position - spawnPosition).normalized;
                Vector3 randomOffset = Random.insideUnitSphere;
                
                Vector3 direction = Vector3.Slerp(baseDirection, randomOffset,
                    Random.Range(0f, Mathf.Tan(_spawnAngle * Mathf.Deg2Rad)));
                direction = direction.normalized;
                return new AsteroidModel(spawnPosition, direction, _enemyConfig.asteroid.speed, _enemyConfig.asteroid.size);
            
            case EnemyType.Ufo:
                var ufoSpawnPosition = Random.onUnitSphere * _worldConfig.worldSize;
                return new UfoModel(ufoSpawnPosition, _playerShip);
            
            default:
                throw new System.ArgumentException($"Unknown enemy type: {type}");
        }
    }
}
