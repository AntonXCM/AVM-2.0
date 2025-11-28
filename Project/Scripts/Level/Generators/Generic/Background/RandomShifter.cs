using Godot;
public partial class RandomShifter : Sprite2D
{
    [Export] private int length;
    public override void _Ready() => RegionRect = RegionRect with { Position = new(GD.Randi() % length, 0) };
}