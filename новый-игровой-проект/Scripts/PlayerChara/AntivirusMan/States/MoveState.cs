using Godot;
using DustyStudios.AVM2.StateMachine;

namespace DustyStudios.AVM2.PlayerChara;
public partial class MoveState : PhysicsState
{
	Vector2 targetVelocity;
	[Export] float speed, velocityChange;
	public override void _Ready()
	{
		base._Ready();
		//Вызывается при обновлении списка нажатых кнопок
		OnUpdate += UpdateAction;
		OnPhysics += PhysicsAction;
	}
    public void PhysicsAction(double delta) => Rb.Velocity = Rb.Velocity.MoveToward(targetVelocity, (float)delta * velocityChange);

    void UpdateAction() => targetVelocity = InputSystem.GetHoryzontalVelocity(speed).Rotated(-Rb.GetFloorAngle()); // GetHoryzontalVelocity - Даёт нажатые кнопки A и D как X в Vector2 //После этого вектор вращается относительно пола
}
