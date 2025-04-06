using UnityEngine;

public class ShipModel
{
    public Vector3 Position { get; private set; }
    public Vector3 Velocity { get; private set; }
    public Quaternion Rotation { get; private set; }
    public int Health { get; private set; }

    private readonly PlayerConfig _config;

    public ShipModel()
    {
        _config = ConfigLoader.LoadPlayerConfig();
        Position = Vector3.zero;
        Rotation = Quaternion.identity;
        Health = _config.maxHealth;
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
