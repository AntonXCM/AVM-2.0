using DustyStudios.AVM2.StateMachine;
using Godot;
using System;

namespace DustyStudios.AVM2.PlayerChara;
public partial class JumpState : PhysicsState
{
	[Export] EmptyState idleState;
	[Export] int speed, force;
	[Export] float jumpStartPower, jumpLeftover, speedDecreace;
	[Export] Vector2 wallClimbing;
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
		if (InputSystem.IsPressed("Space"))
		{
			Rb.Velocity += Vector2.Up * jumpStartPower;
			startedWithSpace = true;
		}
		constantForce = default;
	}
	public void PhysicsAction(double delta)
	{
		if(constantForce.Y < 0)
			constantForce += (float)delta * Rb.GetGravity();
		Rb.Velocity = new((Rb.Velocity.X + constantForce.X) * (float)Mathf.Pow(speedDecreace,delta),Rb.Velocity.Y + constantForce.Y);
		if(Rb.IsOnFloor())
			stateMachine.SetState(idleState);
		else if(Rb.IsOnWall() && InputSystem.IsPressed("Space"))
		{
			var bounce = new Vector2(Rb.GetWallNormal().X, Rb.Velocity.Length()) * wallClimbing;

            Rb.Velocity += bounce;
			constantForce *= Vector2.Down;
		}
	}

	void UpdateAction()
	{
		constantForce.X = (InputSystem.IsPressed("A") ^ InputSystem.IsPressed("D")) ? (InputSystem.IsPressed("A") ? -speed : speed) : 0;
		constantForce.Y = InputSystem.IsPressed("Space") ? (Rb.IsOnFloor() ? (-force) : constantForce.Y) : Mathf.Max(constantForce.Y,0);
		if(!InputSystem.IsPressed("Space") && startedWithSpace)
			Rb.Velocity = new(Rb.Velocity.X, Mathf.Min(Rb.Velocity.Y, CalculateJumpLeftover(Rb.Velocity.Y)));


		float CalculateJumpLeftover(float vel) => startedWithSpace ? (Mathf.Exp(-vel * vel) - Mathf.Max(vel,0)) * jumpLeftover : vel;
	}
}
