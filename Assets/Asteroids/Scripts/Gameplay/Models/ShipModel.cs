using UnityEngine;
using System;

public class ShipModel
{
    public Vector3 Position { get; private set; }
    public Vector3 Velocity { get; private set; }
    public Quaternion Rotation { get; private set; }
    public int Health { get; private set; }
    public bool IsInvulnerable { get; private set; }
    public int LaserCharges { get; private set; }
    public float LastLaserUseTime { get; private set; }

    public event Action OnDeath;
    public event Action<int> OnHealthChanged;
    public event Action<int> OnLaserChargesChanged;

    private readonly PlayerConfig _config;
    private float _invulnerabilityTimer;
    private float _fireTimer;
    private float _laserRechargeTimer;

    public ShipModel()
    {
        _config = ConfigLoader.LoadPlayerConfig();
        Position = Vector3.zero;
        Rotation = Quaternion.identity;
        Health = _config.maxHealth;
        LaserCharges = _config.laserCharges;
        OnHealthChanged?.Invoke(Health);
        OnLaserChargesChanged?.Invoke(LaserCharges);
    }

    public void Move(float thrust)
    {
        Vector3 direction = Rotation * Vector3.forward;
        Velocity += direction * thrust * _config.acceleration * Time.deltaTime;
        Velocity = Vector3.ClampMagnitude(Velocity, _config.maxSpeed);

        if (thrust <= 0f)
        {
            Velocity = Vector3.Lerp(Velocity, Vector3.zero, _config.drag * Time.deltaTime);
        }
        
        Position += Velocity * Time.deltaTime;

        WrapPosition();
    }
    
    public void Rotate(Vector3 rotationInput)
    {
        Vector3 rotationDelta = rotationInput * _config.rotationSpeed * Time.deltaTime;
        Rotation *= Quaternion.Euler(rotationDelta);
    }

    public void ShipUpdate()
    {
        if (IsInvulnerable)
        {
            _invulnerabilityTimer -= Time.deltaTime;
            if (_invulnerabilityTimer <= 0f)
            {
                IsInvulnerable = false;
            }
        }
        _fireTimer += Time.deltaTime;

        if (LaserCharges < _config.laserCharges)
        {
            _laserRechargeTimer -= Time.deltaTime;
            if (_laserRechargeTimer <= 0f)
            {
                LaserCharges++;
                _laserRechargeTimer = _config.laserRechargeTime;
                OnLaserChargesChanged?.Invoke(LaserCharges);
            }
        }
    }

    public bool CanShoot()
    {
        return _fireTimer >= 1f / _config.fireRate;
    }

    public void ResetFireTimer()
    {
        _fireTimer = 0f;
    }

    public bool CanShootLaser()
    {
        return LaserCharges > 0;
    }

    public void UseLaserCharge()
    {
        LaserCharges--;
        LastLaserUseTime = Time.time;
        OnLaserChargesChanged?.Invoke(LaserCharges);
        if (LaserCharges == _config.laserCharges - 1)
        {
            _laserRechargeTimer = _config.laserRechargeTime;
        }
    }

    public void TakeDamage(Vector3 collisionDirection)
    {
        if (IsInvulnerable) return;
        
        Health--;
        OnHealthChanged?.Invoke(Health);
        if (Health <= 0)
        {
            OnDeath?.Invoke();
        }
        
        Velocity = Vector3.Reflect(Velocity, collisionDirection.normalized) * 0.5f;
        IsInvulnerable = true;
        _invulnerabilityTimer = _config.invulnerabilityDuration;
    }

    private void WrapPosition()
    {
        float size = ConfigLoader.LoadWorldConfig().worldSize;
        Position = new Vector3(Wrap(Position.x, size), Wrap(Position.y, size), Wrap(Position.z, size));
    }

    private float Wrap(float value, float limit)
    {
        if (value > limit) return -limit;
        if (value < -limit) return limit;
        return value;
    }
}
