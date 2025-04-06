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
    public float speed;
    public float size;
}

[Serializable]
public class UfoConfig
{
    public float speed;
}