using UnityEngine;

public static class ConfigLoader
{
    public static PlayerConfig LoadPlayerConfig()
    {
        var json = Resources.Load("Configs/PlayerConfig") as TextAsset;
        return JsonUtility.FromJson<PlayerConfig>(json.text);
    }

    public static EnemyConfig LoadEnemyConfig()
    {
        var json = Resources.Load("Configs/EnemyConfig") as TextAsset;
        return JsonUtility.FromJson<EnemyConfig>(json.text);
    }

    public static WorldConfig LoadWorldConfig()
    {
        var json = Resources.Load("Configs/WorldConfig") as TextAsset;
        return JsonUtility.FromJson<WorldConfig>(json.text);
    }
}
