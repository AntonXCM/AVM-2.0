using Godot;

using System;
using System.Collections.Generic;
namespace DustyStudios.AVM2.StateMachine;

public partial class EmptyState : Node, IState
{
	protected StateMachine stateMachine;
	public event Action OnEnter, OnExit;
	public event Action<HashSet<string>> OnUpdate;
	public void Enter() => OnEnter?.Invoke();
	public void Exit() => OnExit?.Invoke();
	public virtual void Update(HashSet<string> keys) => OnUpdate?.Invoke(keys);
	//keys contains the names of all pressed keys.
	public override void _Ready()
	{
		stateMachine = GetParent<StateMachine>();
	}
	public bool IsPlaying { get; set; }
}
