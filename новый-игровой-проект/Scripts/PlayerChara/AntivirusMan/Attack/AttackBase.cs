using DustyStudios.AVM2.PlayerChara;
using Godot;

public abstract partial class AttackBase : Node2D
{
    [Export] public float Delay;
    [Export] public int AmmoCost;
    public AttackState State;
    public abstract void PerformAttack();
    public abstract void EndAttack();
}
