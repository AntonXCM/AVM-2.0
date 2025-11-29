using DustyStudios.AVM2.StateMachine;
using Godot;
namespace DustyStudios.AVM2.PlayerChara;

public partial class AttackTransition : Transition
{
	[Export] InputSystem inputSystem;
	protected override void SetState(EmptyState state)
	{
		state.OnUpdate += () =>
		{
            if (inputSystem.IsPressed("Left") && GlobalStats.Energy.Value > 0)
				DoTransition();
		};
	}
}
