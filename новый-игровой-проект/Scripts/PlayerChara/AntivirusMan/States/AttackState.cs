using System.Collections.Generic;
using DustyStudios.AVM2.StateMachine;
using Godot;

namespace DustyStudios.AVM2.PlayerChara;
public partial class AttackState : IdleState, IRequireEnergy
{
	[Export] AttackBase[] attacks;
	IEnumerator<AttackBase> attacksE;
	float attackTimer;
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
		InitAttacksEnumerator();
		attackTimer = attacksE.Current.Delay;
	}

	void ExitAction() => canExitState = false;
	public override void Update()
	{
		if (canExitState)
			base.Update();
	}
	public override void _Process(double delta)
	{
		if (!IsPlaying) return;
		attackTimer -= (float)delta;

		if (attackTimer > 0) return;
		else if (canExitState || !InputSystem.IsPressed("Left")) stateMachine.SetState(idleState);
		attacksE.Current.EndAttack();

		if (!attacksE.MoveNext())
			InitAttacksEnumerator();
		else AttackCurrent();
	}
	private void AttackCurrent()
	{
		if (Energy.Value - attacksE.Current.AmmoCost < 0)
		{
			canExitState = true;
			return;
		}
		Energy.Value -= attacksE.Current.AmmoCost;
		attackTimer = attacksE.Current.Delay;
		attacksE.Current.PerformAttack();
	}
}
