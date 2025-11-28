using Godot;
public partial class MailmanAnimation : Sprite2D
{
    [Export] public float Sensitivity = 2f, AutoSpeed = 0.2f;
    float prevY, frameTime;

    public override void _Ready() => prevY = GlobalPosition.Y;

    public override void _Process(double delta)
    {
        if ((frameTime += (float)delta) > AutoSpeed)
            frameTime -= AutoSpeed;
        else if (Mathf.Abs(GlobalPosition.Y - prevY) < Sensitivity)
            return;
        Frame = (Frame + 1) % Hframes;
        prevY = GlobalPosition.Y;
    }
}