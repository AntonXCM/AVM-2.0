using DustyStudios.MathAVM;
using Godot;
public partial class CameraBehavior : Node2D
{
    [Export] public Node2D Following;
    [Export] public Vector2 DeadZone;
    [Export] public CollisionShape2D[] StaticScenes;
    [Export] public float Speed;
    public override void _PhysicsProcess(double delta)
    {
        Vector2 followingGlobal = Following.GlobalPosition, global = GlobalPosition;
        float lerpWeight = Mathf.Clamp((float)delta * Speed, 0, 1);
        foreach (var scene in StaticScenes)
            if (MathA.IsPointInCollisionShape(scene, followingGlobal))
            {
                GlobalPosition = global.Lerp(scene.GlobalPosition, lerpWeight);
                return;
            }
        
        GlobalPosition = global.Lerp(followingGlobal.MoveToward(global, DeadZone), lerpWeight);
    }
}
