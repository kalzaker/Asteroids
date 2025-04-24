using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyPool
{
    private readonly List<IEnemy> _enemies = new List<IEnemy>();
    private readonly EnemyFactory _factory;
    private readonly AsteroidView.Factory _asteroidViewFactory;
    private readonly UfoView.Factory _ufoViewFactory;
    private readonly Dictionary<IEnemy, MonoBehaviour> _modelToView = new Dictionary<IEnemy, MonoBehaviour>();
    private readonly int _maxEnemies;
    private readonly ShipModel _shipModel;

    [Inject]
    public EnemyPool(EnemyFactory factory, AsteroidView.Factory asteroidViewFactory, UfoView.Factory ufoViewFactory, ShipModel shipModel, int startSize = 10, int maxEnemies = 30)
    {
        _factory = factory;
        _asteroidViewFactory = asteroidViewFactory;
        _ufoViewFactory = ufoViewFactory;
        _shipModel = shipModel;
        _maxEnemies = maxEnemies;
        Debug.Log($"EnemyPool: Initializing with startSize={startSize}, maxEnemies={maxEnemies}");

        for (int i = 0; i < startSize; i++)
        {
            var enemy = _factory.Create(EnemyType.Asteroid);
            enemy.Deactivate();
            _enemies.Add(enemy);
        }
        Debug.Log($"EnemyPool: Initialized with {_enemies.Count} enemies");
    }

    public IEnemy Get(Vector3? position = null, EnemyType type = EnemyType.Asteroid)
    {
        if (_enemies.Count >= _maxEnemies)
        {
            Debug.LogWarning($"EnemyPool: Max enemies ({_maxEnemies}) reached");
            return null;
        }

        IEnemy enemy = null;
        foreach (var e in _enemies)
        {
            if (!e.IsActive && (type == EnemyType.Asteroid ? e is AsteroidModel : e is UfoModel))
            {
                enemy = e;
                break;
            }
        }

        Vector3 spawnPosition = position ?? Random.onUnitSphere * ConfigLoader.LoadWorldConfig().worldSize;
        Vector3 direction = type == EnemyType.Asteroid ?
            Quaternion.Euler(0, Random.Range(-45f, 45f), 0) * (_shipModel.Position - spawnPosition).normalized :
            (_shipModel.Position - spawnPosition).normalized;

        if (enemy == null)
        {
            enemy = _factory.Create(type, spawnPosition);
            if (enemy != null)
            {
                _enemies.Add(enemy);
            }
        }
        else
        {
            enemy.Activate(spawnPosition, direction);
        }

        if (enemy != null && !_modelToView.ContainsKey(enemy))
        {
            MonoBehaviour view = type == EnemyType.Asteroid ?
                _asteroidViewFactory.Create(enemy as AsteroidModel) :
                _ufoViewFactory.Create(enemy as UfoModel);
            if (view == null)
            {
                Debug.LogError($"EnemyPool: Failed to create view for {type} at {enemy.Position}");
            }
            else
            {
                _modelToView[enemy] = view;
                Debug.Log($"EnemyPool: Created view for {type} at {enemy.Position}");
            }
        }
        else if (enemy != null)
        {
            var view = _modelToView[enemy];
            view.gameObject.SetActive(true);
            view.transform.position = enemy.Position;
            Debug.Log($"EnemyPool: Reused view for {type} at {enemy.Position}");
        }

        return enemy;
    }

    public void Update()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy.IsActive)
            {
                enemy.Update();
                if (_modelToView.ContainsKey(enemy))
                {
                    var view = _modelToView[enemy];
                    view.gameObject.SetActive(enemy.IsActive);
                    if (enemy.IsActive)
                    {
                        view.transform.position = enemy.Position;
                    }
                }
            }
        }
    }

    public IReadOnlyList<IEnemy> GetActive()
    {
        var active = new List<IEnemy>();
        foreach (var enemy in _enemies)
        {
            if (enemy.IsActive)
            {
                active.Add(enemy);
            }
        }
        return active;
    }

    public void Clear()
    {
        foreach (var enemy in _enemies)
        {
            enemy.Deactivate();
            if (_modelToView.ContainsKey(enemy))
            {
                Object.Destroy(_modelToView[enemy].gameObject);
            }
        }
        _modelToView.Clear();
        _enemies.Clear();
        Debug.Log("EnemyPool: Cleared");
    }
}