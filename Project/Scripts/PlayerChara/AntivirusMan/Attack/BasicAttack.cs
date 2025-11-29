using Godot;
public partial class BasicAttack : Attack
{
	[Export] float moveWhileAttack, moveAfterAttack, gravityCountering;
	[Export] InputSystem input;
    void SetVelocity(float speed) => State.Rb.Velocity = input.GetVelocity() * speed + State.Rb.GetGravity() * gravityCountering;
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
