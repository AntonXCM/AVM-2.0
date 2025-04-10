using DustyStudios.MathAVM;
using Godot;
using System;

public partial class PcController : RigidBody2D
{
    [Export] RayCast2D ray;
    [Export] float rayLength = 1, xDispersion, ySpeed = 0.4f, xSpeed = 0.1f, angularSpeed = 10;
    float startXPosition, targetXPosition;
    public float DistanceFactor { get; private set; }
    public override void _Ready()
    {
        startXPosition = Position.X;
        xDispersion *= 2;
        SetXTarget();

        rayLength *= ray.TargetPosition.Length();
    }
    public override void _PhysicsProcess(double delta)
    {
        DistanceFactor = ray.IsColliding() ? Math.Abs(rayLength-(ray.Position - ray.GetCollisionPoint()).Length()) / rayLength : 0;
        float distaceFromXtarget = targetXPosition - Position.X;
        switch (distaceFromXtarget)
        {
            case float a when a < -xSpeed:
                MoveX(-xSpeed);
                break;
            case float a when a > xSpeed:
                MoveX(xSpeed);
                break;
            default:
                SetXTarget();
                break;
        }
        KeepHeight();
        AngularVelocity = MathA.Angle.MoveTowardsDiff(Rotation, 0, (float)delta * angularSpeed);
    }
    void SetXTarget()
    {
        targetXPosition = startXPosition + (GD.Randf() - .5f) * xDispersion;
    }
    public void KeepHeight()
    {
        if (DistanceFactor == 0) return;
        LinearVelocity += ray.TargetPosition.Rotated(ray.GlobalRotation).Normalized() * ySpeed * DistanceFactor;
    }
    public void MoveX(float speed)
    {
        if (DistanceFactor == 0) return;
        LinearVelocity += Vector2.Right * speed * DistanceFactor;
    }
}