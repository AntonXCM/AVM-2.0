using DustyStudios.AVM2.StateMachine;

using Godot;

using System;
using System.Collections.Generic;

namespace DustyStudios.AVM2.PlayerChara;
public partial class JumpState : PhysicsState
{
	[Export] EmptyState idleState;
	[Export] int speed, force;
	[Export] float jumpStartPower, jumpLeftover, wallBouncyness, wallclimbing, speedDecreace;
	bool startedWjthSpace;
	Vector2 constantForce,jumpForceDecrease;
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
		startedWjthSpace = false;
	}
	void EnterAction()
	{
		if(stateMachine.GetKeys().Contains("Space"))
		{
			Rb.Velocity += Vector2.Up * jumpStartPower;
			startedWjthSpace = true;
		}
		jumpForceDecrease = Rb.GetGravity();
	}
	public void PhysicsAction(double delta)
	{
		if(constantForce.Y < 0)
			constantForce += (float)delta * jumpForceDecrease;
		Rb.Velocity = new((Rb.Velocity.X + constantForce.X) * (float)Mathf.Pow( speedDecreace,delta),Rb.Velocity.Y + constantForce.Y);
		if(Rb.IsOnFloor()) stateMachine.SetState(idleState);
		else if(Rb.IsOnWall() && stateMachine.GetKeys().Contains("Space"))
		{
			var bounce = Rb.GetWallNormal()*wallBouncyness*Mathf.Abs(Rb.Velocity.X)+Vector2.Up*wallclimbing*Mathf.Abs(Rb.Velocity.Y*Rb.Velocity.X);
            Rb.Velocity += bounce;
			constantForce *= Vector2.Down;
		}
	}

	void UpdateAction(HashSet<string> keys)
	{
		constantForce.X = (keys.Contains("A") ^ keys.Contains("D")) ? (keys.Contains("A") ? -speed : speed) : 0;
		constantForce.Y = keys.Contains("Space") ? (Rb.IsOnFloor() ? (-force) : constantForce.Y) : Mathf.Max(constantForce.Y,0);
		if(!keys.Contains("Space")&&startedWjthSpace&&false)
			Rb.Velocity = new(Rb.Velocity.X, Mathf.Min(Rb.Velocity.Y, CalculateJumpLeftover(Rb.Velocity.Y)));


		float CalculateJumpLeftover(float vel) => startedWjthSpace ? (Mathf.Exp(-vel * vel) - Mathf.Max(vel,0)) * jumpLeftover : vel;
	}
}
