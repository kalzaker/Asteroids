using JetBrains.Annotations;
using UnityEngine;

public class LaserModel
{
    public Vector3 StartPosition { get; private set; }
    public Vector3 Direction { get; private set; }
    public bool IsActive { get; private set; }
    public float Duration { get; private set; } = 0.2f;

    private float _timer;

    public LaserModel(Vector3 startPosition, Quaternion rotation)
    {
        StartPosition = startPosition;
        Direction = rotation * Vector3.forward;
        IsActive = true;
        _timer = Duration;
    }

    public void LaserUpdate()
    {
        if (!IsActive) return;
        
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            Deactivate();
        }
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Reset(Vector3 startPosition, Quaternion rotation)
    {
        StartPosition = startPosition;
        Direction = rotation * Vector3.forward;
        IsActive = true;
        _timer = Duration;
    }
}