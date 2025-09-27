using Godot;

public partial class ShieldCharacter : Node2D, IHasRigidBody2D
{
    public CharacterBody2D Rb { get; set; }
    [Export] float bounceForce = 300, degThrehold = 60;
    float cosThrehold = 0.5f;
    public override void _Ready()
    {
        cosThrehold = Mathf.Cos(degThrehold);
    }
    Vector2 normal;
    public override void _Draw()
    {
        DrawLine(Position, Position + normal, Colors.Red, 2, true);
    }


    public override void _PhysicsProcess(double delta)
    {
        Vector2 shieldDirection = InputSystem.GetWASD();

        if (shieldDirection == Vector2.Zero)
            return;
        shieldDirection = shieldDirection.Normalized();

        int count = Rb.GetSlideCollisionCount();
        for (int i = 0; i < count; i++)
        {
            var col = Rb.GetSlideCollision(i);
            if (col is null) continue;

            var normal = col.GetNormal();
            if (Mathf.Abs(shieldDirection.Dot(-normal)) >= cosThrehold)
                continue;
            this.normal = normal;
            if (col.GetCollider() is RigidBody2D rb)
                rb.ApplyCentralImpulse(-shieldDirection * bounceForce);
            else
                Rb.Velocity += -shieldDirection * bounceForce;
            return;
        }

    }
}