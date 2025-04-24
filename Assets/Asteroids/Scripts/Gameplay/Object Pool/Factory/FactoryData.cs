using System;
using System.Collections.Generic;

[Serializable]
public class FactoryObjData<T> where T : class
{
    public string Key;
    public ObjectPool<T> ObjectPool;
}

public class FactoryData<T> where T : class
{
    public int MaxCountInstance = 30;
    public int LifeTime = 10;
    public List<FactoryObjData<T>> FactoryObjData = new List<FactoryObjData<T>>();

    public FactoryObjData<T> GetDataFromKey(string key)
    {
        var data = FactoryObjData.Find(x => x.Key == key);
        if (data == null)
        {
            throw new Exception($"FactoryData: No data for key {key}");
        }
        return data;
    }
}