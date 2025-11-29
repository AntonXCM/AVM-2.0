using DustyStudios.AVM2.StateMachine;

using Godot;

namespace DustyStudios.AVM2.PlayerChara;
public partial class MoveTransition : Transition
{
	[Export] bool isEnter;
	[Export] InputSystem inputSystem;
	protected override void SetState(EmptyState state)
	{
		state.OnUpdate += () =>
		{
			if (inputSystem.HasHoryzontal() == isEnter) DoTransition(); 
		};
	}
}
