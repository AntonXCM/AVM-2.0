using DustyStudios.AVM2.StateMachine;
using Godot;

namespace DustyStudios.AVM2.PlayerChara;

public partial class JumpTransition : Transition
{
	[Export] InputSystem inputSystem;
	protected override void SetState(EmptyState state)
	{
		state.OnUpdate += () =>
		{
			if(inputSystem.IsPressed("Space") && !inputSystem.IsPressed("S")) DoTransition();
		};

		if(!(state is PhysicsState)) return;
		PhysicsState pState = (PhysicsState)state;

		pState.OnPhysics += d =>
		{
			if(!pState.Rb.IsOnFloor()) DoTransition();
		};
	}
}