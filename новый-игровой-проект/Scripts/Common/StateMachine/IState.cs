using System.Collections.Generic;
namespace DustyStudios.AVM2.StateMachine;

public interface IState
{
	void Enter();   
	void Update(HashSet<string> keys); 
	void Exit();
	bool IsPlaying { get; set; }
}
