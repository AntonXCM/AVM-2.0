using DustyStudios.AVM2.PlayerChara;
using Godot;
using System.Collections.Generic;

public abstract partial class AttackBase : Node2D
{
    [Export] public float Delay;
    [Export] public int AmmoCost;
    public AttackState State;
    public HashSet<string> Keys;
    public abstract void PerformAttack();
    public abstract void EndAttack();
}
