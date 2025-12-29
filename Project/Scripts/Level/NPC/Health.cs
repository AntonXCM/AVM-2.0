using System;
using Godot;
using Godot.Collections;

public partial class Health : Node, IDamagable, IHealable, IKillable
{
    [Export] private float health;
    [Export] AudioStreamPlayer2D deathSound, punchSound;
    [Export] Dictionary<IDamagable.DamageType, float> damageTypeMultipliers;
    public Action OnDeath, OnDamage;
    public virtual void Damage(float damage, IDamagable.DamageType type)
    {
        if (damageTypeMultipliers.ContainsKey(type))
            health -= damage * damageTypeMultipliers[type];
        else
            health -= damage;
        OnDamage?.Invoke();
        
        if (health <= 0)
            Kill();
        else
            punchSound?.Play();
    }
    public void Heal(float healing) => health += healing;

    public virtual void Kill()
    {
        OnDeath?.Invoke();
        deathSound?.Play();
    }
}