using System.Collections.Generic;
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
    Vector2 shieldDebug, forceDebug;
    HashSet<Vector2> normalsDebug = [];
    public override void _Draw()
    {
        foreach (var normal in normalsDebug)
            DrawLine(Vector2.Zero, normal * 22, Colors.OrangeRed, 2);

        DrawLine(Vector2.Zero, shieldDebug * 22, Colors.LightBlue, 2);
        DrawLine(Vector2.Zero, forceDebug / 10, Colors.LimeGreen, 2);
    }


    public override void _PhysicsProcess(double _)
    {
        Vector2 shieldDirection = InputSystem.GetWASD();

        if (shieldDirection == Vector2.Zero)
            return;
        shieldDirection = shieldDirection.Normalized();

        int count = Rb.GetSlideCollisionCount();
        if(count > 0)
            normalsDebug.Clear();
        for (int i = 0; i < count; i++)
        {
            var collision = Rb.GetSlideCollision(i);

            var normal = collision.GetNormal();
            normalsDebug.Add(normal);
            if (-shieldDirection.Dot(normal) < cosThrehold)
                continue;
            if (collision.GetCollider() is RigidBody2D rb)
            {
                forceDebug = shieldDirection * bounceForce;
                rb.ApplyImpulse(shieldDirection * bounceForce, collision.GetPosition());
                Rb.Velocity += -shieldDirection * selfBounce;
            }
            else if(shieldDirection.Y != 0)
            {
                forceDebug = -shieldDirection * selfBounce;   
                Rb.Velocity += -shieldDirection * selfBounce;
            }
            break;
        }
        shieldDebug = shieldDirection;
        QueueRedraw();

    }
}