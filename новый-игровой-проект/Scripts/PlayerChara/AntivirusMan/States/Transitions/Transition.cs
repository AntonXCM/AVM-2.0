using Godot;
namespace DustyStudios.AVM2.StateMachine;
public abstract partial class Transition : Node
{
	StateMachine stateMachine;
	protected EmptyState targetState;
	[Export] EmptyState[] entranceStates;
	public override void _Ready()
	{
		targetState = GetParent<EmptyState>();
		stateMachine = targetState.GetParent<StateMachine>();
		foreach (var state in entranceStates)
			SetState(state);
	}
	abstract protected void SetState(EmptyState state);
    public void DoTransition() => stateMachine.SetState(targetState);
}
