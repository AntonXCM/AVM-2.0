using DustyStudios.AVM2.StateMachine;
namespace DustyStudios.AVM2.PlayerChara;

public partial class AttackTransition : Transition
{
	protected override void SetState(EmptyState state)
	{
		state.OnUpdate += () =>
		{
            if (InputSystem.IsPressed("Left") && (targetState as AttackState).Energy.Value > 0)
				DoTransition();
		};
	}
}
