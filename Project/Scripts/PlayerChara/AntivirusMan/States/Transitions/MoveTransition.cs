using DustyStudios.AVM2.StateMachine;

using Godot;

namespace DustyStudios.AVM2.PlayerChara;
public partial class MoveTransition : Transition
{
	[Export] bool isEnter;
	protected override void SetState(EmptyState state)
	{
		state.OnUpdate += () =>
		{
			if (InputSystem.HasHoryzontal() == isEnter) DoTransition(); 
		};
	}
}
