using Godot;
using System.Collections.Generic;

namespace DustyStudios.AVM2.StateMachine;
public partial class StateMachine : Node
{
	IState current;
	[Export] EmptyState initialState;
	HashSet<string> keys = new();
	public override void _Ready()
	{
		SetState(initialState);
	}
	public void SetState(IState newState)
	{
		current?.Exit();
		if(current != null) current.IsPlaying = false;
		current = newState;
		current.IsPlaying = true;
		current?.Enter();
		current.Update(keys);
	}
	public override void _Input(InputEvent Event)
	{
		if(current == null) return;
		if(Event is not InputEventKey) return;
		string key = Event.AsText();
		if(!Event.IsPressed())
		{
			keys.Remove(key);
		}
		else
		{
			if(keys.Contains(key)) return;
			keys.Add(key); 
		}
		current.Update(GetKeys());
	}
	public HashSet<string> GetKeys() => new(keys);
}
