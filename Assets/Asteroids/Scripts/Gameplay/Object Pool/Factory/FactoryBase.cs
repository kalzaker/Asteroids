using UnityEngine;
using Zenject;

public class FactoryBase<T> : IFactory<T>, IInitializable where T : class
{
    private int _countInstance;
    protected int _lifeTime;
    private DiContainer _diContainer;
    private FactoryData<T> _data;

    public DiContainer DiContainer => _diContainer;
    public FactoryData<T> FactoryData => _data;

    [Inject]
    public FactoryBase(FactoryData<T> data, DiContainer diContainer)
    {
        _lifeTime = data.LifeTime;
        _countInstance = data.MaxCountInstance;
        _data = data;
        _diContainer = diContainer;

        foreach (var objData in _data.FactoryObjData)
        {
            objData.ObjectPool = new ObjectPool<T>(
                obj => OnInstantiateObj(obj),
                (obj, pos) => OnGetObj(obj, pos),
                obj => OnReturnObj(obj)
            );
        }
    }

    public void Initialize()
    {
        foreach (var objData in _data.FactoryObjData)
        {
            for (int i = 0; i < _countInstance; i++)
            {
                objData.ObjectPool.InstantiateToPool(Instantiate(objData.Key));
            }
        }
        Debug.Log($"FactoryBase<{typeof(T).Name}>: Initialized with {_countInstance} instances per key");
    }

    public virtual void OnInstantiateObj(T obj)
    {
        Debug.Log($"FactoryBase: Instantiated {typeof(T).Name}");
    }

    public virtual void OnGetObj(T obj, Vector3 position)
    {
        Debug.Log($"FactoryBase: Got {typeof(T).Name} at {position}");
    }

    public virtual void OnReturnObj(T obj)
    {
        Debug.Log($"FactoryBase: Returned {typeof(T).Name}");
    }

    public T Create(string key, Vector3 position)
    {
        T obj = _data.GetDataFromKey(key).ObjectPool.GetFromPool(position);
        return obj;
    }

    public T Instantiate(string key)
    {
        T instance;
        if (typeof(T) == typeof(AsteroidModel))
        {
            instance = new AsteroidModel() as T;
        }
        else if (typeof(T) == typeof(UfoModel))
        {
            instance = new UfoModel() as T;
        }
        else
        {
            Debug.LogError($"FactoryBase: Unsupported type {typeof(T).Name}");
            return default;
        }

        OnInstantiateObj(instance);
        return instance;
    }

    protected void ReturnToPool(T obj, string key)
    {
        _data.GetDataFromKey(key).ObjectPool.ReturnToPool(obj);
    }
}

public interface IFactory<T> where T : class
{
    DiContainer DiContainer { get; }
    FactoryData<T> FactoryData { get; }
    T Instantiate(string key);
}