using System.Collections.Generic;
namespace DustyStudios.AVM2.StateMachine;

public interface IState
{
	void Enter();   
	void Update(); 
	void Exit();
	bool IsPlaying { get; set; }
}
