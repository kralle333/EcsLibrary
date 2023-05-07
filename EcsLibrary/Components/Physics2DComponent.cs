using Microsoft.Xna.Framework;

namespace EcsLibrary.Components;

public class Physics2DComponent : Component
{
    public Vector2 Velocity { get; private set; }
    public Vector2 Acceleration { get; private set; }
    private readonly float _maxVelocity;
    private readonly float _maxAcceleration;

    public Physics2DComponent(float maxVelocity = 0, float maxAcceleration = 0)
    {
        _maxVelocity = maxVelocity;
        _maxAcceleration = maxAcceleration;
    }

    private Vector2 CorrectIfAboveMax(Vector2 value, float max)
    {
        var length = value.Length();
        if (length > max)
        {
            value = (value / length) * max;
        }

        return value;
    }

    public void SetAcceleration(Vector2 acc)
    {
        Acceleration = CorrectIfAboveMax(acc, _maxAcceleration);
    }
    public void AddAcceleration(Vector2 acc)
    {
        SetAcceleration(Acceleration + acc);
    }

    public void SetVelocity(Vector2 vel)
    {
        Velocity = CorrectIfAboveMax(vel, _maxVelocity);
    }
    public void AddVelocity(Vector2 vel)
    {
        SetVelocity(Velocity+vel);
    }

    public void Update()
    {
        AddVelocity(Acceleration);
    }

    public void StopVelocity()
    {
        Velocity = Vector2.Zero;
    }

    public void StopAcceleration()
    {
        Acceleration = Vector2.Zero;
    }
}