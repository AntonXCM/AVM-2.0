using Godot;
public partial class WindowGraphics : Sprite2D
{
    [Export] private float maxOffset;
    public override void _Ready() => SetInstanceShaderParameter("offset", GD.Randf() * maxOffset);
}