using DustyStudios.AVM2.StateMachine;

using Godot;
using System;

namespace DustyStudios.AVM2.PlayerChara;

public partial class JumpTransition : Transition
{
	protected override void SetState(EmptyState state)
	{
		state.OnUpdate += k =>
		{
			if(k.Contains("Space")) DoTransition();
		};

		if(!(state is PhysicsState)) return;
		PhysicsState pState = (PhysicsState)state;

		pState.OnPhysics += d =>
		{
			if(!pState.Rb.IsOnFloor()) DoTransition();
		};
	}
}