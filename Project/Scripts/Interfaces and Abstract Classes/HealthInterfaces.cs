public interface IDamagable
{
    public enum DamageType
    {
        Default,
        Explosion,
        Enviroment,
    }
    void Damage(float damage, DamageType type = DamageType.Default);
}
public interface IHealable
{
    void Heal(float hp);
}
public interface IKillable
{
    void Kill();
}
public interface IRespawn
{
    void Respawn(float seconds);
}