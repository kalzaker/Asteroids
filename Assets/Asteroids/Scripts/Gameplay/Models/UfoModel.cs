using UnityEngine;

public class UfoModel : IEnemy
{
    public Vector3 Position { get; private set; }
    public Vector3 Velocity { get; private set; }
    public bool IsActive { get; private set; }
    public EnemyType Type => EnemyType.Ufo;

    private readonly ShipModel _target;
    private readonly float _speed;
    private readonly WorldConfig _worldConfig;

    public UfoModel(Vector3 spawnPosition, ShipModel target)
    {
        Position = spawnPosition;
        _target = target;
        _worldConfig = ConfigLoader.LoadWorldConfig();
        _speed = ConfigLoader.LoadEnemyConfig().ufo.speed;
        IsActive = true;
    }

    public void PositionUpdate()
    {
        if (!IsActive) return;
        
        Vector3 direction = (_target.Position - Position).normalized;
        Velocity = direction * _speed;
        Position += Velocity * Time.deltaTime;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
