using UnityEngine;
using Zenject;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private readonly EnemyFactory _enemyFactory;
    private readonly BulletFactory _bulletFactory;
    private float _spawnTimer;
    private readonly float _spawnInterval = 2f;
    private readonly List<IEnemy> _activeEnemies = new List<IEnemy>();

    [Inject]
    public GameManager(EnemyFactory enemyFactory, BulletFactory bulletFactory)
    {
        _enemyFactory = enemyFactory;
        _bulletFactory = bulletFactory;
        Debug.Log($"GameManager: Initialized, enemyFactory={(_enemyFactory != null)}, bulletFactory={(_bulletFactory != null)}");
    }

    private void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0)
        {
            string key = Random.value < 0.8f ? "Asteroid" : "Ufo";
            var enemy = _enemyFactory.Create(key, Random.onUnitSphere * ConfigLoader.LoadWorldConfig().worldSize);
            _activeEnemies.Add(enemy);
            _spawnTimer = _spawnInterval;
            Debug.Log($"GameManager: Spawned {key}");
        }

        // Обновление врагов и спавн фрагментов
        for (int i = _activeEnemies.Count - 1; i >= 0; i--)
        {
            var enemy = _activeEnemies[i];
            enemy.Update(Time.deltaTime);
            if (!enemy.IsActive && enemy.Type == EnemyType.Asteroid && !(enemy as AsteroidModel).IsFragment)
            {
                float fragmentSize = (enemy as AsteroidModel).Size * 0.5f;
                for (int j = 0; j < 2; j++)
                {
                    Vector3 fragmentPos = enemy.Position + Random.insideUnitSphere * fragmentSize;
                    var fragment = _enemyFactory.Create("Asteroid", fragmentPos, fragmentSize, true);
                    _activeEnemies.Add(fragment);
                    Debug.Log($"GameManager: Spawned fragment at {fragmentPos}, size={fragmentSize}");
                }
                _activeEnemies.RemoveAt(i);
                _enemyFactory.ReturnToPool(enemy, "Asteroid");
            }
            else if (!enemy.IsActive)
            {
                _activeEnemies.RemoveAt(i);
                _enemyFactory.ReturnToPool(enemy, enemy.Type == EnemyType.Asteroid ? "Asteroid" : "Ufo");
            }
        }

        _enemyFactory.UpdateViews();
        _bulletFactory.UpdateViews();

        // Тестовый урон (нажми Q)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            foreach (var enemy in _activeEnemies)
            {
                if (enemy.Type == EnemyType.Asteroid)
                {
                    enemy.TakeDamage(1);
                    Debug.Log($"GameManager: Dealt 1 damage to {enemy.Type} at {enemy.Position}");
                }
            }
        }
    }

    public void Shoot(Vector3 position, Vector3 direction)
    {
        _bulletFactory.Create("Bullet", position, direction);
        Debug.Log($"GameManager: Fired bullet at {position}");
    }
}