using Godot;
using DustyStudios.AVM2.StateMachine;
namespace DustyStudios.AVM2.PlayerChara;

public partial class IdleState : PhysicsState
{
	Vector2 origVelocity;
	[Export] int friction;
    public override void _Ready()
    {
        base._Ready();
		OnPhysics += PhysicsAction;
		OnEnter += EnterAction;
	}
	void EnterAction()
	{
		origVelocity = Rb.Velocity;
	}
    public void PhysicsAction(double delta)
	{
		if (friction == 0) return;
		origVelocity = new(interpolate(origVelocity.X),interpolate(origVelocity.Y));
		float interpolate(float n)
		{
			if(n == 0) return 0;
			float fr = (float)delta * friction;
			bool wasPositive = n > 0;
			if (n < 0)
				n += fr;
			else
				n -= fr;
			if(wasPositive ^ n > 0)
				return 0;
			return n;
		}
		Rb.Velocity = origVelocity;
	}
}
