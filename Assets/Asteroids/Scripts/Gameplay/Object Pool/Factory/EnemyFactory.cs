using UnityEngine;
using Zenject;

public class EnemyFactory : FactoryBase<IEnemy>
{
    private readonly ShipModel _shipModel;
    private readonly float _worldSize;
    private readonly AsteroidView.Factory _asteroidViewFactory;
    private readonly UfoView.Factory _ufoViewFactory;
    private readonly Dictionary<IEnemy, MonoBehaviour> _modelToView = new Dictionary<IEnemy, MonoBehaviour>();

    [Inject]
    public EnemyFactory(FactoryData<IEnemy> data, DiContainer diContainer, ShipModel shipModel, AsteroidView.Factory asteroidViewFactory, UfoView.Factory ufoViewFactory)
        : base(data, diContainer)
    {
        _shipModel = shipModel;
        _asteroidViewFactory = asteroidViewFactory;
        _ufoViewFactory = ufoViewFactory;
        var config = ConfigLoader.LoadWorldConfig();
        _worldSize = config.worldSize;
        Debug.Log($"EnemyFactory: Initialized, worldSize={_worldSize}");
    }

    public override void OnInstantiateObj(IEnemy obj)
    {
        MonoBehaviour view = obj.Type == EnemyType.Asteroid ?
            _asteroidViewFactory.Create(obj as AsteroidModel) :
            _ufoViewFactory.Create(obj as UfoModel);
        if (view == null)
        {
            Debug.LogError($"EnemyFactory: Failed to create view for {obj.Type}");
        }
        else
        {
            _modelToView[obj] = view;
            view.gameObject.SetActive(false);
            Debug.Log($"EnemyFactory: Created view for {obj.Type}");
        }
    }

    public override void OnGetObj(IEnemy obj, Vector3 position)
    {
        Vector3 direction = obj.Type == EnemyType.Asteroid ?
            Quaternion.Euler(0, Random.Range(-45f, 45f), 0) * (_shipModel.Position - position).normalized :
            (_shipModel.Position - position).normalized;

        float size = ConfigLoader.LoadEnemyConfig().asteroidSize;
        bool isFragment = false;

        if (obj is AsteroidModel asteroid && _modelToView[obj].transform.localScale.x < size)
        {
            size = _modelToView[obj].transform.localScale.x;
            isFragment = true;
        }

        obj.Activate(position, direction, size, isFragment);
        if (_modelToView.ContainsKey(obj))
        {
            var view = _modelToView[obj];
            view.gameObject.SetActive(true);
            view.transform.position = obj.Position;
            if (obj is AsteroidModel)
            {
                view.transform.localScale = Vector3.one * size;
            }
        }
    }

    public override void OnReturnObj(IEnemy obj)
    {
        obj.Deactivate();
        if (_modelToView.ContainsKey(obj))
        {
            _modelToView[obj].gameObject.SetActive(false);
        }
    }

    public IEnemy Create(string key, Vector3 position, float size = 1f, bool isFragment = false)
    {
        IEnemy enemy = base.Create(key, position);
        if (_modelToView.ContainsKey(enemy))
        {
            var view = _modelToView[enemy];
            view.transform.localScale = Vector3.one * size;
        }
        return enemy;
    }

    public void UpdateViews()
    {
        foreach (var pair in _modelToView)
        {
            var enemy = pair.Key;
            var view = pair.Value;
            view.gameObject.SetActive(enemy.IsActive);
            if (enemy.IsActive)
            {
                view.transform.position = enemy.Position;
            }
        }
    }
}