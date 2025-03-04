using DustyStudios.AVM2.StateMachine;
using Godot;
using System;
using System.Collections.Generic;

namespace Tests;
public partial class TestState : EmptyState
{

	public override void Update(HashSet<string> keys) => GD.Print(String.Join(", ", keys));

	public override void _Ready()
	{
		base._Ready();
		stateMachine.SetState(this);
	}
}
