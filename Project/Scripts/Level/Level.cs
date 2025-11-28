using Godot;

public partial class Level : Node
{
    [Export] public Vector2 Speed;
    private double startTime;

    public double TimeSpent => Time.GetTicksMsec() / 1000.0 - startTime;

    public override void _Ready() => startTime = Time.GetTicksMsec() / 1000.0;
}
