using DustyStudios.AVM2.PlayerChara;

using Godot;
public partial class BasicAttack : Attack
{
	[Export] float moveWhileAttack, moveAfterAttack, gravityCountering;

    void SetVelocity(float speed) => State.Rb.Velocity = MoveState.GetVelocity(Keys) * speed + State.Rb.GetGravity() * gravityCountering;
    public override void PerformAttack()
	{
		base.PerformAttack();
		SetVelocity(moveWhileAttack);
	}
	public override void EndAttack()
	{
		base.EndAttack();
		SetVelocity(moveAfterAttack);
	}
}
