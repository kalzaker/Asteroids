using UnityEngine;
using Zenject;

public class BulletFactory : FactoryBase<BulletModel>
{
    private readonly BulletView.Factory _bulletViewFactory;
    private readonly Dictionary<BulletModel, BulletView> _modelToView = new Dictionary<BulletModel, BulletView>();

    [Inject]
    public BulletFactory(FactoryData<BulletModel> data, DiContainer diContainer, BulletView.Factory bulletViewFactory)
        : base(data, diContainer)
    {
        _bulletViewFactory = bulletViewFactory;
        Debug.Log("BulletFactory: Initialized");
    }

    public override void OnInstantiateObj(BulletModel obj)
    {
        var view = _bulletViewFactory.Create();
        if (view == null)
        {
            Debug.LogError("BulletFactory: Failed to create BulletView");
        }
        else
        {
            view.Initialize(obj);
            _modelToView[obj] = view;
            view.gameObject.SetActive(false);
            Debug.Log("BulletFactory: Created BulletView");
        }
    }

    public override void OnGetObj(BulletModel obj, Vector3 position)
    {
        obj.Activate(position, Vector3.zero);
        if (_modelToView.ContainsKey(obj))
        {
            var view = _modelToView[obj];
            view.gameObject.SetActive(true);
            view.transform.position = obj.Position;
        }
    }

    public override void OnReturnObj(BulletModel obj)
    {
        obj.Deactivate();
        if (_modelToView.ContainsKey(obj))
        {
            _modelToView[obj].gameObject.SetActive(false);
        }
    }

    public BulletModel Create(string key, Vector3 position, Vector3 direction)
    {
        BulletModel bullet = base.Create(key, position);
        bullet.Activate(position, direction);
        if (_modelToView.ContainsKey(bullet))
        {
            _modelToView[bullet].transform.position = bullet.Position;
        }
        return bullet;
    }

    public void UpdateViews()
    {
        foreach (var pair in _modelToView)
        {
            var bullet = pair.Key;
            var view = pair.Value;
            view.gameObject.SetActive(bullet.IsActive);
            if (bullet.IsActive)
            {
                view.transform.position = bullet.Position;
            }
        }
    }
}