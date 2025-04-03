using UnityEngine;

public class AsteroidModel : IEnemy
{
    public Vector3 Position { get; private set; }
    public Vector3 Velocity { get; private set; }
    public float Size { get; private set; }
    public bool IsActive { get; private set; }
    public EnemyType Type => EnemyType.Asteroid;

    public AsteroidModel(Vector3 spawnPosition, Vector3 direction, float speed, float size)
    {
        Position = spawnPosition;
        Velocity = direction.normalized * speed;
        Size = size;
        IsActive = true;
    }

    public void PositionUpdate()
    {
        if (!IsActive) return;
        
        Position += Velocity * Time.deltaTime;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
