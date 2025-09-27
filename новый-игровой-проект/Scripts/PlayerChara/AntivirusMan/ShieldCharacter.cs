using Godot;

public partial class ShieldCharacter : Node2D, IHasRigidBody2D
{
    public CharacterBody2D Rb { get; set; }
    [Export] float selfBounce = 2222, bounceForce = 300, degThrehold = 60;
    float cosThrehold = 0.5f;
    public override void _Ready()
    {
        cosThrehold = Mathf.Cos(degThrehold);
    }
    Vector2 normalDebug, shieldDebug;
    public override void _Draw()
    {
        DrawLine(Vector2.Zero, normalDebug * 22, Colors.OrangeRed, 2, true);
        DrawLine(Vector2.Zero, shieldDebug * 22, Colors.LightBlue, 2, true);
    }


    public override void _PhysicsProcess(double _)
    {
        Vector2 shieldDirection = InputSystem.GetWASD();

        if (shieldDirection == Vector2.Zero)
            return;
        shieldDirection = shieldDirection.Normalized();

        int count = Rb.GetSlideCollisionCount();
        for (int i = 0; i < count; i++)
        {
            var col = Rb.GetSlideCollision(i);

            var normal = col.GetNormal();
            if (-shieldDirection.Dot(normal) < cosThrehold)
                continue;
            normalDebug = normal;
            shieldDebug = shieldDirection;
            QueueRedraw();
            if (col.GetCollider() is RigidBody2D rb)
                rb.ApplyCentralImpulse(shieldDirection * bounceForce);
            else
                Rb.Velocity += -shieldDirection * selfBounce;
            return;
        }

    }
}