using System;

[Serializable]
public class EnemyConfig
{
    public AsteroidConfig asteroid;
    public UfoConfig ufo;
}

[Serializable]
public class AsteroidConfig
{
    public float minSpeed;
    public float maxSpeed;
    public float size;
    public int health;
    public float fragmentMinSpeed;
    public float fragmentMaxSpeed;
    public float fragmentSize;
    public int minFragments;
    public int maxFragments;
    public float spawnInterval;
}

[Serializable]
public class UfoConfig
{
    public float speed;
    public int health;
    public float spawnInterval;
}