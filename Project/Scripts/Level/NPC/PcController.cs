using DustyStudios.MathAVM;
using Godot;

public partial class PcController : RigidBody2D
{
    [Export] public bool AIEnabled;
    [Export] RayCast2D ray;
    [Export] float xDispersion, xSpeed = 0.1f, angularSpeed = 10, ySpeed = 0.4f;
    float startXPosition, targetXPosition, rayLength;
    public float DistanceFactor { get; private set; }
    public override void _Ready()
    {
        startXPosition = Position.X;
        xDispersion *= 2;
        SetXTarget();

        rayLength = ray.TargetPosition.Length();
        AddDrag();
    }
    public void AddDrag()
    {
        var drag = this.GetGrandchild<Drag>();
        drag.OnStart += () => AIEnabled = false;
        drag.OnEnd += () => AIEnabled = true;
        this.GetGrandchild<Respawn>().OnRespawn += drag.EndDragging;
    }
    public override void _PhysicsProcess(double delta)
    {
        DistanceFactor = ray.IsColliding() ? rayLength - (ray.GlobalPosition - ray.GetCollisionPoint()).Length() : 0;
        float distaceFromXtarget = targetXPosition - Position.X;
        float deltaf = (float)delta;
        KeepHeight(deltaf);
        if (AIEnabled)
        {
            switch (distaceFromXtarget)
            {
                case float a when a < -xSpeed:
                    MoveX(deltaf, -xSpeed);
                    break;
                case float a when a > xSpeed:
                    MoveX(deltaf, xSpeed);
                    break;
                default:
                    SetXTarget();
                    break;
            }
            AngularVelocity = MathA.Angle.MoveTowardsDiff(Rotation, 0, deltaf * angularSpeed);
        }
    }
    void SetXTarget() => targetXPosition = startXPosition + (GD.Randf() - .5f) * xDispersion;
    public void KeepHeight(float delta)
    {
        if (DistanceFactor is 0) return;
        LinearVelocity += ray.TargetPosition.Rotated(ray.GlobalRotation).Normalized() * ySpeed * DistanceFactor * delta;
    }
    public void MoveX(float delta, float speed)
    {
        if (DistanceFactor is 0) return;
        LinearVelocity += Vector2.Right * speed * DistanceFactor * delta;
    }
}