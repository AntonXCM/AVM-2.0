using DustyStudios.AVM2.StateMachine;

using Godot;
using System;

namespace DustyStudios.AVM2.PlayerChara;
public partial class MoveTransition : Transition
{
	[Export] bool isEnter;
	protected override void SetState(EmptyState state)
	{
		state.OnUpdate += k =>
		{
			if(k.Contains("A") ^ k.Contains("D") == isEnter) DoTransition(); 
		};
		GD.Print(isEnter + "Setted move state");
	}
}
