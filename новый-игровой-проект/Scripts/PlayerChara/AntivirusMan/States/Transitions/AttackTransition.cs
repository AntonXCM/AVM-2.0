using DustyStudios.AVM2.StateMachine;
namespace DustyStudios.AVM2.PlayerChara;

public partial class AttackTransition : Transition
{
	protected override void SetState(EmptyState state)
	{
		state.OnUpdate += k =>
		{
            IEnergy energy = (targetState as AttackState).Energy;
            if (k.Contains("Left") && energy.Value > 0)
				DoTransition();
		};
	}
}
