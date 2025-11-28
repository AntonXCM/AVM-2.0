using Godot;
public partial class RailGlint : Sprite2D
{
    [Export] float frameTime = 0.2f;
    [Export] Vector2 posBounds;
    float leftFrameTime = 0;
    public override void _Process(double delta)
    {
        if ((leftFrameTime -= (float)delta) > 0) return;
        leftFrameTime += frameTime;
        int frame = Frame + 1;
        if (frame >= (Vframes * Hframes))
        {
            Frame = 0;
            Position = Position with { X = Mathf.Lerp(posBounds.X, posBounds.Y, GD.Randf()) };
        }
        else
            Frame = frame;
    }

}
