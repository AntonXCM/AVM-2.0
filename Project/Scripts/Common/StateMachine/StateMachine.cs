using Godot;

namespace DustyStudios.AVM2.StateMachine;
public partial class StateMachine : Node
{
	IState current;
	[Export] EmptyState initialState;
	[Export] InputSystem inputSystem;
	public override void _Ready()
	{
		SetState(initialState);
		inputSystem.OnInput += Input;
	}
    protected override void Dispose(bool disposing)
    {
		base.Dispose(disposing);
		inputSystem.OnInput -= Input;
    }

    public void SetState(IState newState)
	{
		current?.Exit();
		if (current != null) current.IsPlaying = false;
		current = newState;
		current.IsPlaying = true;
		current?.Enter();
		current.Update();
	}
    public void Input() => current.Update();
}
