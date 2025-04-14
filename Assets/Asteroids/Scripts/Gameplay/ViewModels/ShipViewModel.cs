using UnityEngine;

public class ShipViewModel
{
    private readonly ShipModel _shipModel;
    private readonly InputController _inputController;
    private readonly BulletPool _bulletPool;
    private readonly LaserPool _laserPool;
    
    public Vector3 Position => _shipModel.Position;
    public Quaternion Rotation => _shipModel.Rotation;
    public bool IsInvulnerable => _shipModel.IsInvulnerable;

    public ShipViewModel(ShipModel shipModel, InputController inputController, BulletPool bulletPool, LaserPool laserPool)
    {
        _shipModel = shipModel;
        _inputController = inputController;
        _bulletPool = bulletPool;
        _laserPool = laserPool;
    }

    public void Update()
    {
        _shipModel.Move(_inputController.GetThrustInput());
        _shipModel.Rotate(_inputController.GetRotationInput());
        _shipModel.ShipUpdate();

        if (_inputController.IsShooting() && _shipModel.CanShoot())
        {
            _bulletPool.Get();
            _shipModel.ResetFireTimer();
        }

        if (_inputController.IsShootingLaser() && _shipModel.CanShootLaser())
        {
            _laserPool.Get();
            _shipModel.UseLaserCharge();
        }
    }

    public void HandleCollision(IEnemy enemy)
    {
        Vector3 collisionDirection = (_shipModel.Position - enemy.Position).normalized;
        _shipModel.TakeDamage(collisionDirection);
    }
}
