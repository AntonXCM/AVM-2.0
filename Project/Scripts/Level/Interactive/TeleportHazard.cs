using Godot;
public partial class TeleportHazard : Area2D
{
    [Export] float respawnTime;
    public override void _Ready() => BodyEntered += Respawn;

    public void Respawn(Node target)
    {

        if (target.TryGetGrandchild<IRespawn>(out var respawn))
            respawn.Respawn(respawnTime);
        else if (target.TryGetGrandchild<IKillable>(out var killable))
            killable.Kill();
    }

}
