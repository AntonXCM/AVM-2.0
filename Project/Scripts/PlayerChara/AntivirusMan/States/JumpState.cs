using DustyStudios.AVM2.StateMachine;
using Godot;
using Godot.Collections;

namespace DustyStudios.AVM2.PlayerChara;
public partial class JumpState : PhysicsState
{
	[Export] EmptyState idleState;
	[Export] int speed, force;
	[Export] float jumpStartPower, jumpLeftover, speedDecreace;
	[Export] AudioStreamPlayer2D landSound;
	[Export] AudioStream defaultSound;
	[Export] Dictionary<string, AudioStream> audios;
	[Export] InputSystem inputSystem;
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
		landSound.Play();
	}
	void EnterAction()
	{
		if (Rb.Velocity.Y > 0)
			Rb.Velocity *= Vector2.Right;
		if (inputSystem.IsPressed("Space"))
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
	}

	void UpdateAction()
	{
		if (!startedWithSpace) return;
		constantForce.X = inputSystem.GetHoryzontalVelocity() * speed;
		constantForce.Y = inputSystem.IsPressed("Space") ? (Rb.IsOnFloor() ? (-force) : constantForce.Y) : Mathf.Max(constantForce.Y,0);
		if(!inputSystem.IsPressed("Space"))
			Rb.Velocity = new(Rb.Velocity.X, Mathf.Min(Rb.Velocity.Y, CalculateJumpLeftover(Rb.Velocity.Y)));


		float CalculateJumpLeftover(float vel) => startedWithSpace ? (Mathf.Exp(-vel * vel) - Mathf.Max(vel,0)) * jumpLeftover : vel;
	}
}
