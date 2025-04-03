using UnityEngine;

public interface IEnemy
{
    Vector3 Position { get; }
    Vector3 Velocity { get; }
    bool IsActive { get; }
    EnemyType Type { get; }
    void PositionUpdate();
    void Deactivate();
}

public enum EnemyType
{
    Asteroid,
    Ufo
}