using System.Collections.Generic;
using UnityEngine;

public class BulletPool
{
    private readonly List<BulletModel> _bullets;
    private readonly ShipModel _ship;
    private readonly int _initialCapacity;

    public BulletPool(ShipModel ship, int capacity = 20)
    {
        _ship = ship;
        _initialCapacity = capacity;
        _bullets = new List<BulletModel>(capacity);

        for (int i = 0; i < capacity; i++)
        {
            var bullet = new BulletModel(Vector3.zero, Quaternion.identity);
            bullet.Deactivate();
            _bullets.Add(bullet);
        }
    }

    public BulletModel Get()
    {
        foreach (var bullet in _bullets)
        {
            if (!bullet.IsActive)
            {
                bullet.Reset(_ship.Position, _ship.Rotation);
                return bullet;
            }
        }
        
        var newBullet = new BulletModel(_ship.Position, _ship.Rotation);
        _bullets.Add(newBullet);
        return newBullet;
    }

    public void UpdateBullet()
    {
        foreach (var bullet in _bullets)
        {
            if (bullet.IsActive)
            {
                bullet.PositionUpdate();
            }
        }
    }

    public List<BulletModel> GetActiveBullets()
    {
        return _bullets.FindAll(b => b.IsActive);
    }
}