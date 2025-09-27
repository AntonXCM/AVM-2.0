using Godot;
using DustyStudios.AVM2.StateMachine;

namespace DustyStudios.AVM2.PlayerChara;
public partial class MoveState : PhysicsState
{
	[Export] int speed;
	public override void _Ready()
	{
		base._Ready();
		OnUpdate += UpdateAction;
	}
	void UpdateAction() => Rb.Velocity = InputSystem.GetHoryzontalVelocity(speed).Rotated(-Rb.GetFloorAngle());
}
