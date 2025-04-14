using UnityEngine;

public class EnemyFactory
{
    private readonly ShipModel _playerShip;
    private readonly EnemyConfig _enemyConfig;
    private readonly WorldConfig _worldConfig;
    private readonly EnemyPool _enemyPool;
    private float _spawnAngle = 45f;

    public EnemyFactory(ShipModel playerShip, EnemyPool enemyPool)
    {
        _playerShip = playerShip;
        _enemyPool = enemyPool;
        _enemyConfig = ConfigLoader.LoadEnemyConfig();
        _worldConfig = ConfigLoader.LoadWorldConfig();
    }

    public IEnemy Create(EnemyType type, Vector3? spawnPosition = null, bool isFragment = false)
    {
        switch (type)
        {
            case EnemyType.Asteroid:
                Vector3 position = spawnPosition ?? Random.onUnitSphere * _worldConfig.worldSize;
                Vector3 baseDirection = spawnPosition.HasValue
                    ? Random.insideUnitSphere.normalized
                    : (_playerShip.Position - position).normalized;
                Vector3 randomOffset = Random.insideUnitSphere;
                Vector3 direction = Vector3.Slerp(baseDirection, randomOffset,
                    Random.Range(0f, Mathf.Tan(_spawnAngle * Mathf.Deg2Rad))).normalized;

                float speed, size;
                if (isFragment)
                {
                    speed = Random.Range(_enemyConfig.asteroid.fragmentMinSpeed, _enemyConfig.asteroid.fragmentMaxSpeed);
                    size = _enemyConfig.asteroid.fragmentSize;
                }
                else
                {
                    speed = Random.Range(_enemyConfig.asteroid.minSpeed, _enemyConfig.asteroid.maxSpeed);
                    size = _enemyConfig.asteroid.size;
                }
                return new AsteroidModel(position, direction, speed, size, _enemyPool, isFragment);
            
            case EnemyType.Ufo:
                var ufoSpawnPosition = spawnPosition ?? Random.onUnitSphere * _worldConfig.worldSize;
                return new UfoModel(ufoSpawnPosition, _playerShip);
            
            default:
                throw new System.ArgumentException($"Unknown enemy type: {type}");
        }
    }
}
