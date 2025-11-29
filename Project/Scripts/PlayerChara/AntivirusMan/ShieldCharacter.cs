using Godot;

public partial class ShieldCharacter : Node, IHasRigidBody2D
{
    public CharacterBody2D Rb { get; set; }
    [Export] float selfBounce, bounceForce, degThrehold;
	[Export] InputSystem inputSystem;
    float cosThrehold = 0.5f;
    public override void _Ready() => cosThrehold = Mathf.Cos(degThrehold);
    public override void _PhysicsProcess(double _)
    {
        Vector2 shieldDirection = inputSystem.GetWASD();

        if (shieldDirection == Vector2.Zero)
            return;
        shieldDirection = shieldDirection.Normalized();

        int count = Rb.GetSlideCollisionCount();
        for (int i = 0; i < count; i++)
        {
            var collision = Rb.GetSlideCollision(i);

            var normal = collision.GetNormal();
            if (-shieldDirection.Dot(normal) >= cosThrehold)
            {
                if (collision.GetCollider() is RigidBody2D rb)
                {
                    rb.ApplyImpulse(shieldDirection * bounceForce, collision.GetPosition());
                    Rb.Velocity += -shieldDirection * selfBounce;
                }
                else if (shieldDirection.Y != 0)
                    Rb.Velocity += -shieldDirection * selfBounce;
                break;
            }
        }
    }
}