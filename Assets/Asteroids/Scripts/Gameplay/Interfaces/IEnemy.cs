using UnityEngine;

public interface IEnemy
{
    Vector3 Position { get; }
    bool IsActive { get; }
    EnemyType Type { get; }
    void Deactivate();
}

public enum EnemyType
{
    Asteroid,
    Ufo
}