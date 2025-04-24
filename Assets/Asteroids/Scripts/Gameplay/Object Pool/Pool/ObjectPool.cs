using System;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPoolObject
{
    void OnReset();
}

public sealed class ObjectPool<T> where T : class
{
    private Queue<T> _inactiveObjects;
    private List<T> _activeObjects;

    private event Action<T> onInstantiate;
    private event Action<T, Vector3> onGet;
    private event Action<T> onReturn;

    public ObjectPool(Action<T> OnInstantiate, Action<T, Vector3> OnGet, Action<T> OnReturn)
    {
        _inactiveObjects = new Queue<T>();
        _activeObjects = new List<T>();
        onInstantiate = OnInstantiate;
        onGet = OnGet;
        onReturn = OnReturn;
    }

    public T GetFromPool(Vector3 position)
    {
        if (_inactiveObjects.Count == 0)
        {
            if (_activeObjects.Count == 0)
            {
                Debug.LogError("ObjectPool: No objects available in pool.");
                return default;
            }
            else
            {
                T obj = _activeObjects[0];
                _activeObjects.RemoveAt(0);
                _inactiveObjects.Enqueue(obj);
                Debug.LogWarning("ObjectPool: Reusing active object due to empty inactive pool");
            }
        }

        T _obj = _inactiveObjects.Dequeue();
        _activeObjects.Add(_obj);
        onGet?.Invoke(_obj, position);
        return _obj;
    }

    public void ReturnToPool(T obj)
    {
        if (_activeObjects.Contains(obj))
        {
            _activeObjects.Remove(obj);
            _inactiveObjects.Enqueue(obj);
            onReturn?.Invoke(obj);
            ResetObj(obj);
        }
        else
        {
            Debug.LogError($"ObjectPool: Object {obj} not found in active pool");
        }
    }

    public void InstantiateToPool(T obj)
    {
        onInstantiate?.Invoke(obj);
        _inactiveObjects.Enqueue(obj);
    }

    private void ResetObj(T obj)
    {
        if (obj is IObjectPoolObject poolObj)
        {
            poolObj.OnReset();
        }
    }
}