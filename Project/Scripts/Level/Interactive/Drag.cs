using System;
using Godot;

public partial class Drag : Node2D
{
    [Export] float energyDecaySpeed = 0.5f, scrollSpeed = Mathf.Pi;
    [Export(PropertyHint.Range, "0, 1")] float linearInertia = 0.4f, rotationInertia = 0.5f;
    [Export] CollisionObject2D pickCollision;
    bool isDragging;
    RigidBody2D rb;
    Vector2 lastVector;
    float targetRotation = 0;
    public event Action OnStart, OnEnd;
    public override void _Ready()
    {
        rb = GetParent<RigidBody2D>();
        pickCollision.InputEvent += (_, e, _) => InputEvent(e);
    }
    public void InputEvent(InputEvent e)
    {
        if (e is InputEventMouseButton mb && mb.Pressed && mb.ButtonIndex is MouseButton.Left)
            StartDragging();
    }

    public override void _Input(InputEvent e)
    {
        if (e is InputEventMouseButton mb)
        {
            if (mb.ButtonIndex is MouseButton.Left && !mb.Pressed)
                EndDragging();

            if (isDragging)
            {
                if (mb.ButtonIndex is MouseButton.WheelUp)
                    targetRotation = (targetRotation + scrollSpeed) % Mathf.Tau;
                else if (mb.ButtonIndex is MouseButton.WheelDown)
                    targetRotation = (targetRotation - scrollSpeed) % Mathf.Tau;
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!isDragging) return;
        rb.LinearVelocity = rb.LinearVelocity.Lerp((GetGlobalMousePosition() - rb.GlobalPosition) / (float)delta, linearInertia);
        rb.AngularVelocity = Mathf.Lerp(rb.AngularVelocity, Mathf.AngleDifference(rb.GlobalRotation, targetRotation) / (float)delta, rotationInertia);
    }

    public void StartDragging()
    {
        if (isDragging) return; 
        targetRotation = rb.GlobalRotation;
        isDragging = true;
        OnStart?.Invoke();
        AmmoDecay();
    }

    public void EndDragging()
    {
        if (!isDragging) return; 
        isDragging = false;
        OnEnd?.Invoke();
    }

    async void AmmoDecay()
    {
        while (isDragging)
        {
            if (GlobalStats.Energy.Value is 0)
            {
                EndDragging();
                return;
            }
            GlobalStats.Energy.Value--;
            await ToSignal(GetTree().CreateTimer(energyDecaySpeed), "timeout");
        }
    }
}
