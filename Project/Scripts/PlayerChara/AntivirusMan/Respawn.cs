using System;
using Godot;
public partial class Respawn : Node, IRespawn
{
    Vector2 mainCheckpoint;
    public event Action OnRespawn;
    public override void _Ready() => mainCheckpoint = GetParent<Node2D>().Position;
    async void IRespawn.Respawn(float seconds)
    {
        await ToSignal(GetTree().CreateTimer(seconds), Timer.SignalName.Timeout);
        var parent = GetParent<Node2D>();
        parent.Position = mainCheckpoint;
        if (parent is CharacterBody2D chara) chara.Velocity = default;
        else if (parent is RigidBody2D rigidBody) rigidBody.LinearVelocity = default;
        OnRespawn?.Invoke();
    }
}