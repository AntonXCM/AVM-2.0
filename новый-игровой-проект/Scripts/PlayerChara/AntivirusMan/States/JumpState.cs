using DustyStudios.AVM2.StateMachine;
using Godot;
using System;

namespace DustyStudios.AVM2.PlayerChara;
public partial class JumpState : PhysicsState
{
	[Export] EmptyState idleState;
	[Export] int speed, force;
	[Export] float jumpStartPower, jumpLeftover, speedDecreace;
	bool startedWithSpace;
	Vector2 constantForce;
	public override void _Ready()
	{
		base._Ready();
		OnEnter += EnterAction;
		OnUpdate += UpdateAction;
		OnExit += ExitAction;
		OnPhysics += PhysicsAction;
	}
	void ExitAction()
	{
		Rb.Velocity *= Vector2.Right;
		startedWithSpace = false;
	}
	void EnterAction()
	{
		GD.Print("Before"+Rb.Velocity);
		if (Rb.Velocity.Y > 0) Rb.Velocity *= Vector2.Right;
		if (InputSystem.IsPressed("Space"))
		{
			Rb.Velocity += Vector2.Up * jumpStartPower;
			startedWithSpace = true;
		}
		GD.Print("After"+Rb.Velocity);
		constantForce = default;
	}
	public void PhysicsAction(double delta)
	{
		if(constantForce.Y < 0)
			constantForce += (float)delta * Rb.GetGravity();
		Rb.Velocity = new((Rb.Velocity.X + constantForce.X) * (float)Mathf.Pow(speedDecreace,delta),Rb.Velocity.Y + constantForce.Y);
		if(Rb.IsOnFloor())
			stateMachine.SetState(idleState);
	}

	void UpdateAction()
	{
		if (!startedWithSpace) return;
		constantForce.X = (InputSystem.IsPressed("A") ^ InputSystem.IsPressed("D")) ? (InputSystem.IsPressed("A") ? -speed : speed) : 0;
		constantForce.Y = InputSystem.IsPressed("Space") ? (Rb.IsOnFloor() ? (-force) : constantForce.Y) : Mathf.Max(constantForce.Y,0);
		if(!InputSystem.IsPressed("Space"))
			Rb.Velocity = new(Rb.Velocity.X, Mathf.Min(Rb.Velocity.Y, CalculateJumpLeftover(Rb.Velocity.Y)));


		float CalculateJumpLeftover(float vel) => startedWithSpace ? (Mathf.Exp(-vel * vel) - Mathf.Max(vel,0)) * jumpLeftover : vel;
	}
}
