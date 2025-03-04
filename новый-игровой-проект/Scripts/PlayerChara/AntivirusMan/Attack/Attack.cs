using Godot;

public partial class Attack : AttackBase
{
	[Export] PackedScene attackScene;
	[Export] Node2D parent;
	public override void PerformAttack()
	{
		Node2D attackInstance = attackScene.Instantiate() as Node2D;
		parent.GetParent().AddChild(attackInstance);
		attackInstance.GlobalPosition = GlobalPosition;
	}

	public override void EndAttack(){}
}
