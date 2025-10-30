using Godot;

public partial class CharacterPhysics : CharacterBody2D
{
    [Export] public float XVelocityCap, YVelocityCap, GravityScale = 1.0f, FloorVelocityFactor = 1;
    public Vector2 FloorVelocity = Vector2.Zero;

    public override void _PhysicsProcess(double delta)
    {
        if (!IsOnFloor())
            Velocity += GetGravity() * (float)delta * GravityScale;
        else if (FloorVelocityFactor != 0)
            for (int i = 0; i < GetSlideCollisionCount(); i++)
            {
                var collision = GetSlideCollision(i);
                var normal = collision.GetNormal();

                float angle = Mathf.Acos(normal.Dot(UpDirection.Normalized()));
                if (angle <= FloorMaxAngle)
                {
                    var floor = collision.GetCollider();
                    FloorVelocity =
                        (floor as Node).TryGetGrandparent<IHasVelocity>(out var velocity) ? velocity.Velocity :
                        default; 
                }
            }
        Velocity = new Vector2(
            Mathf.Clamp(Velocity.X, -XVelocityCap, XVelocityCap),
            Mathf.Clamp(Velocity.Y, -YVelocityCap, YVelocityCap)
        );
        GlobalPosition += FloorVelocity * FloorVelocityFactor * (float)delta;
        MoveAndSlide();
    }
}
