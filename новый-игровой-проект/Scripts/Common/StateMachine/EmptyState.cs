using Godot;
using System;

namespace DustyStudios.AVM2.StateMachine;

public partial class EmptyState : Node, IState
{
	protected StateMachine stateMachine;
	public event Action OnEnter, OnUpdate, OnExit;
	public void Enter() => OnEnter?.Invoke();
	public void Exit() => OnExit?.Invoke();
	public virtual void Update() => OnUpdate?.Invoke();
    public override void _Ready() => stateMachine = GetParent<StateMachine>();
    public bool IsPlaying { get; set; }
}
