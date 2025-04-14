using UnityEngine;

public class UfoModel : IEnemy
{
    public Vector3 Position { get; private set; }
    public Vector3 Velocity { get; private set; }
    public bool IsActive { get; private set; }
    public int Health { get; private set; }
    public EnemyType Type => EnemyType.Ufo;

    private readonly ShipModel _target;
    private readonly float _speed;

    public UfoModel(Vector3 spawnPosition, ShipModel target)
    {
        Position = spawnPosition;
        _target = target;
        _speed = ConfigLoader.LoadEnemyConfig().ufo.speed;
        Health = ConfigLoader.LoadEnemyConfig().ufo.health;
        IsActive = true;
    }

    public void PositionUpdate()
    {
        if (!IsActive) return;
        
        Vector3 direction = (_target.Position - Position).normalized;
        Velocity = direction * _speed;
        Position += Velocity * Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Deactivate();
        }
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
