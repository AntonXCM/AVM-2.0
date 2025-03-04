using Godot;
using DustyStudios.AVM2.StateMachine;
using System.Collections.Generic;

namespace DustyStudios.AVM2.PlayerChara;
public partial class MoveState : PhysicsState
{
	[Export] int speed;
	public override void _Ready()
	{
		base._Ready();
		OnUpdate += UpdateAction;
	}
	void UpdateAction(HashSet<string> keys) => Rb.Velocity = (GetVelocity(keys) * Vector2.Right * speed).Rotated(-Rb.GetFloorAngle());
	public static Vector2 GetVelocity(HashSet<string> keys)
	{
		Vector2 v = Vector2.Zero;
		if (keys.Contains("A") ^ keys.Contains("D"))
			v.X = keys.Contains("A") ? -1 : 1;
		if (keys.Contains("S") ^ keys.Contains("W"))
			v.Y = keys.Contains("W") ? -1 : 1;
		v *= v.Length();
		return v;
	}
}
