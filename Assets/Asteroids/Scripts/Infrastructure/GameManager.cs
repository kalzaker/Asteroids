using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] private EnemyPool _enemyPool;
    [Inject] private AsteroidView.Factory _asteroidViewFactory;
    [Inject] private UfoView.Factory _ufoViewFactory;

    private Dictionary<IEnemy, MonoBehaviour> _enemyViews = new Dictionary<IEnemy, MonoBehaviour>();
    private float _asteroidSpawnTimer = 0f;
    private float _ufoSpawnTimer = 0f;
    private float _asteroidSpawnInterval = 2f;
    private float _ufoSpawnInterval = 5f;

    private void Update()
    {
        _asteroidSpawnTimer += Time.deltaTime;
        if (_asteroidSpawnTimer >= _asteroidSpawnInterval)
        {
            _asteroidSpawnTimer = 0f;
            _enemyPool.Get(EnemyType.Asteroid);
        }
        
        _ufoSpawnTimer += Time.deltaTime;
        if (_ufoSpawnTimer >= _ufoSpawnInterval)
        {
            _ufoSpawnTimer = 0f;
            _enemyPool.Get(EnemyType.Ufo);
        }
        
        _enemyPool.UpdateEnemy();
        
        var activeEnemies = _enemyPool.GetActiveEnemies();
        foreach (var enemy in activeEnemies)
        {
            if (!_enemyViews.ContainsKey(enemy))
            {
                switch (enemy.Type)
                {
                    case EnemyType.Asteroid:
                        var asteroidView = _asteroidViewFactory.Create((AsteroidModel)enemy);
                        _enemyViews[enemy] = asteroidView;
                        break;
                    case EnemyType.Ufo:
                        var ufoView = _ufoViewFactory.Create((UfoModel)enemy);
                        _enemyViews[enemy] = ufoView;
                        break;
                }
            }
        }
        
        CleanupInactive(activeEnemies);
    }

    private void CleanupInactive(List<IEnemy> activeEnemies)
    {
        var toRemove = new List<IEnemy>();
        foreach (var kvp in _enemyViews)
        {
            if (!activeEnemies.Contains(kvp.Key))
            {
                kvp.Value.gameObject.SetActive(false);
                toRemove.Add(kvp.Key);
            }
        }

        foreach (var key in toRemove)
        {
            _enemyViews.Remove(key);
        }
    }
}
