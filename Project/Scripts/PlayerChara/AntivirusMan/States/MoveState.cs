using Godot;
using Godot.Collections;
using DustyStudios.AVM2.StateMachine;

namespace DustyStudios.AVM2.PlayerChara;
public partial class MoveState : PhysicsState
{
	Vector2 targetVelocity;
	[Export] float speed, velocityChange;
	[Export] AudioStreamPlayer2D walkSound;
	[Export] AudioStream defaultSound;
	[Export] Dictionary<string, AudioStream> audios;
	[Export] InputSystem inputSystem;
	public override void _Ready()
	{
		base._Ready();
		OnEnter += () => walkSound.Play();
		OnUpdate += UpdateAction;
		OnPhysics += PhysicsAction;
		OnExit += walkSound.Stop;
	}
	public void PhysicsAction(double delta)
	{
		Rb.Velocity = Rb.Velocity.MoveToward(targetVelocity, (float)delta * velocityChange);
		walkSound.PitchScale = Rb.Velocity.Length() / speed;
	}
	void UpdateAction() => targetVelocity = new Vector2(inputSystem.GetHoryzontalVelocity() * speed,0).Rotated(-Rb.GetFloorAngle());
}
