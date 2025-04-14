using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] private EnemyPool _enemyPool;
    [Inject] private BulletPool _bulletPool;
    [Inject] private LaserPool _laserPool;
    
    [Inject] private AsteroidView.Factory _asteroidViewFactory;
    [Inject] private UfoView.Factory _ufoViewFactory;
    [Inject] private BulletView.Factory _bulletViewFactory;
    [Inject] private LaserView.Factory _laserViewFactory;
    
    [Inject] private ShipModel _shipModel;
    [Inject] private GameOverUI _gameOverUI;
    [Inject] private UIManager _uiManager;

    private Dictionary<IEnemy, MonoBehaviour> _enemyViews = new Dictionary<IEnemy, MonoBehaviour>();
    private Dictionary<BulletModel, BulletView> _bulletViews = new Dictionary<BulletModel, BulletView>();
    private Dictionary<LaserModel, LaserView> _laserViews = new Dictionary<LaserModel, LaserView>();
    
    private float _asteroidSpawnTimer = 0f;
    private float _ufoSpawnTimer = 0f;
    private float _asteroidSpawnInterval;
    private float _ufoSpawnInterval;

    private void Awake()
    {
        _asteroidSpawnInterval = ConfigLoader.LoadEnemyConfig().asteroid.spawnInterval;
        _ufoSpawnInterval = ConfigLoader.LoadEnemyConfig().ufo.spawnInterval;
        _shipModel.OnDeath += HandleShipDestroyed;
    }

    private void OnDestroy()
    {
        _shipModel.OnDeath -= HandleShipDestroyed;
    }

    private void Update()
    {
        _enemyPool.UpdateEnemy();
        _bulletPool.UpdateBullet();
        _laserPool.UpdateLasers();
        
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
        
        var activeBullets = _bulletPool.GetActiveBullets();
        foreach (var bullet in activeBullets)
        {
            if (!_bulletViews.ContainsKey(bullet))
            {
                var bulletView = _bulletViewFactory.Create(bullet);
                _bulletViews[bullet] = bulletView;
            }
        }
        
        var activeLasers = _laserPool.GetActiveLasers();
        foreach (var laser in activeLasers)
        {
            if (!_laserViews.ContainsKey(laser))
            {
                var laserView = _laserViewFactory.Create(laser);
                _laserViews[laser] = laserView;

                RaycastHit hit;
                if (Physics.Raycast(laser.StartPosition, laser.Direction, out hit, 1000f))
                {
                    if (hit.collider.TryGetComponent<AsteroidView>(out var asteroidView))
                    {
                        asteroidView.GetAsteroidModel()?.Deactivate();
                    }
                    else if (hit.collider.TryGetComponent<UfoView>(out var ufoView))
                    {
                        ufoView.GetUfoModel()?.Deactivate();
                    }
                }
            }
        }
        
        CleanupInactiveEnemies(activeEnemies);
        CleanupInactiveBullets(activeBullets);
    }

    private void HandleShipDestroyed()
    {
        PlayerPrefs.Save();
        _gameOverUI.Show(_uiManager.GetScore());
    }

    private void CleanupInactiveEnemies(List<IEnemy> activeEnemies)
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

    private void CleanupInactiveBullets(List<BulletModel> activeBullets)
    {
        var toRemove = new List<BulletModel>();
        foreach (var kvp in _bulletViews)
        {
            if (!activeBullets.Contains(kvp.Key))
            {
                kvp.Value.gameObject.SetActive(false);
                toRemove.Add(kvp.Key);
            }
        }

        foreach (var key in toRemove)
        {
            _bulletViews.Remove(key);
        }
    }

    private void CleanupInactiveLasers(List<LaserModel> activeLasers)
    {
        var toRemove = new List<LaserModel>();
        foreach (var kvp in _laserViews)
        {
            if (!activeLasers.Contains(kvp.Key))
            {
                kvp.Value.gameObject.SetActive(false);
                toRemove.Add(kvp.Key);
            }

            foreach (var key in toRemove)
            {
                _laserViews.Remove(key);
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        float worldSize = ConfigLoader.LoadWorldConfig().worldSize;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(worldSize, worldSize, worldSize));
    }
}
