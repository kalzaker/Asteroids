using System.Collections.Generic;
using UnityEngine;

public class LaserPool
{
    private readonly List<LaserModel> _lasers;
    private readonly ShipModel _ship;
    private readonly int _initialCapacity;

    public LaserPool(ShipModel ship, int capacity = 3)
    {
        _ship = ship;
        _initialCapacity = capacity;
        _lasers = new List<LaserModel>(capacity);

        for (int i = 0; i < capacity; i++)
        {
            var laser = new LaserModel(Vector3.zero, Quaternion.identity);
            laser.Deactivate();
            _lasers.Add(laser);
        }
    }

    public LaserModel Get()
    {
        foreach (var laser in _lasers)
        {
            if (!laser.IsActive)
            {
                laser.Reset(_ship.Position, _ship.Rotation);
                return laser;
            }
        }
        
        var newLaser = new LaserModel(_ship.Position, _ship.Rotation);
        _lasers.Add(newLaser);
        return newLaser;
    }

    public void UpdateLasers()
    {
        foreach (var laser in _lasers)
        {
            if (laser.IsActive)
            {
                laser.LaserUpdate();
            }
        }
    }

    public List<LaserModel> GetActiveLasers()
    {
        return _lasers.FindAll(l => l.IsActive);
    }
}