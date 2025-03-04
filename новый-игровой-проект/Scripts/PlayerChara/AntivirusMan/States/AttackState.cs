using System.Collections.Generic;
using DustyStudios.AVM2.StateMachine;
using Godot;

namespace DustyStudios.AVM2.PlayerChara;
public partial class AttackState : IdleState, IRequireEnergy
{
	[Export] AttackBase[] attacks;
	IEnumerator<AttackBase> attacksE;
	float attackTimer;
	HashSet<string> keys;
	[Export] EmptyState idleState;
	bool canExitState = false;
	public IEnergy Energy { get; set; }
	public override void _Ready()
	{
		base._Ready();
		OnEnter += EnterAction;
		OnExit += ExitAction;
		foreach (AttackBase attack in attacks)
			attack.State = this;
	}
	void InitAttacksEnumerator()
	{
		attacksE = e();

		IEnumerator<AttackBase> e()
		{
			foreach (var attack in attacks)
				yield return attack;
		}

		attacksE.MoveNext();
		AttackCurrent();
	}

	void EnterAction()
	{
		keys = stateMachine.GetKeys();
		InitAttacksEnumerator();
		attackTimer = attacksE.Current.Delay;
	}

	void ExitAction() => canExitState = false;
	public override void Update(HashSet<string> keys)
	{
		if (canExitState) base.Update(keys);
	}
	public override void _Process(double delta)
	{
		if (!IsPlaying) return;
		attackTimer -= (float)delta;

		keys = stateMachine.GetKeys();
		if (attackTimer > 0) return;
		else if (canExitState || !keys.Contains("Left")) stateMachine.SetState(idleState);
		attacksE.Current.EndAttack();

		if (!attacksE.MoveNext())
			InitAttacksEnumerator();
		else AttackCurrent();
	}
	private void AttackCurrent()
	{
		//if (Energy == null) return;
		if (Energy.Value - attacksE.Current.AmmoCost < 0)
		{
			canExitState = true;
			return;
		}
		Energy.Value -= attacksE.Current.AmmoCost;
		attackTimer = attacksE.Current.Delay;
		attacksE.Current.Keys = stateMachine.GetKeys();
		attacksE.Current.PerformAttack();
	}
}
