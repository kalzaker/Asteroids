using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;

public class EnemyPool
{
    private readonly EnemyFactory _factory;
    private readonly List<IEnemy> _enemies;
    private readonly int _maxEnemies;

    public event Action<IEnemy> OnEnemyDeactivated;

    public EnemyPool(EnemyFactory factory, int capacity = 10)
    {
        _factory = factory;
        _maxEnemies = ConfigLoader.LoadWorldConfig().maxEnemies;
        _enemies = new List<IEnemy>(capacity);

        for (var i = 0; i < capacity; i++)
        {
            var enemy = _factory.Create(EnemyType.Asteroid);
            enemy.Deactivate();
            _enemies.Add(enemy);
        }
    }

    public IEnemy Get(EnemyType type, Vector3? spawnPosition = null, bool isFragment = false)
    {
        int activeCount = _enemies.FindAll(e => e.IsActive).Count;
        if (activeCount >= _maxEnemies)
        {
            return null;
        }
            
        foreach (var enemy in _enemies)
        {
            if (!enemy.IsActive)
            {
                var newEnemy = _factory.Create(type, spawnPosition, isFragment);
                var index = _enemies.IndexOf(enemy);
                _enemies[index] = newEnemy;
                return newEnemy;
            }
        }
        
        var extraEnemy  = _factory.Create(type, spawnPosition, isFragment);
        _enemies.Add(extraEnemy);
        return extraEnemy;
    }

    public void SpawnFragments(Vector3 position)
    {
        int fragmentCount = Random.Range(ConfigLoader.LoadEnemyConfig().asteroid.minFragments,
            ConfigLoader.LoadEnemyConfig().asteroid.maxFragments + 1);

        for (int i = 0; i < fragmentCount; i++)
        {
            Get(EnemyType.Asteroid, position, true);
        }
    }

    public void UpdateEnemy()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy.IsActive)
            {
                enemy.PositionUpdate();
            }
            else
            {
                OnEnemyDeactivated?.Invoke(enemy);
            }
        }
    }

    public List<IEnemy> GetActiveEnemies()
    {
        return _enemies.FindAll(e => e.IsActive);
    }
}
