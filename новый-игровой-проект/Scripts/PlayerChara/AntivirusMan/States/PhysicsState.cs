
using Godot;
using System;
namespace DustyStudios.AVM2.StateMachine;
public abstract partial class PhysicsState: EmptyState, IHasRigidBody2D
{
    public event Action<double> OnPhysics;
    public override void _PhysicsProcess(double delta)
    {
        if(!IsPlaying) return;
        OnPhysics?.Invoke(delta);
    }
    public CharacterBody2D Rb { get; set; }
}