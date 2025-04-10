using System.Collections.Generic;

public class EnemyPool
{
    private readonly EnemyFactory _factory;
    private readonly List<IEnemy> _enemies;
    private int _initialCapacity;
    private readonly int _maxEnemies;

    public EnemyPool(EnemyFactory factory, int capacity = 10)
    {
        _factory = factory;
        _maxEnemies = ConfigLoader.LoadWorldConfig().maxEnemies;
        _initialCapacity = capacity;
        _enemies = new List<IEnemy>(capacity);

        for (var i = 0; i < capacity; i++)
        {
            var enemy = _factory.Create(EnemyType.Asteroid);
            enemy.Deactivate();
            _enemies.Add(enemy);
        }
    }

    public IEnemy Get(EnemyType type)
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
                var newEnemy = _factory.Create(type);
                var index = _enemies.IndexOf(enemy);
                _enemies[index] = newEnemy;
                return newEnemy;
            }
        }
        
        var extraEnemy  = _factory.Create(type);
        _enemies.Add(extraEnemy);
        return extraEnemy;
    }

    public void UpdateEnemy()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy.IsActive)
            {
                enemy.PositionUpdate();
            }
        }
    }

    public List<IEnemy> GetActiveEnemies()
    {
        return _enemies.FindAll(e => e.IsActive);
    }
}
