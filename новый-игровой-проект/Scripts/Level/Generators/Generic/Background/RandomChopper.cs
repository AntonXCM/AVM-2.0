using Godot;
public partial class RandomChopper : Sprite2D
{
    [Export] private int sprites;
    public override void _Ready() => RegionRect = RegionRect with { Position = new((GD.Randi() % sprites) * (int)RegionRect.Size.X, 0) };
}