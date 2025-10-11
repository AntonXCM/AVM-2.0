using Godot;

public partial class MouseMove : Node2D
{
    [Export] private Camera2D camera;
    [Export] private float scaleSpeed;
    private bool dragging;
    private Vector2 lastMousePos;

    public override void _UnhandledInput(InputEvent inputEvent)
    {
        if (inputEvent is InputEventMouseButton mb)
        {
            switch (mb.ButtonIndex)
            {
                case MouseButton.Left:
                    if (mb.Pressed)
                    {
                        dragging = true;
                        lastMousePos = mb.Position;
                    }
                    else
                        dragging = false;
                    break;
                case MouseButton.WheelUp:
                    if(camera.Zoom.X < 5)
                        camera.Zoom += Vector2.One * scaleSpeed;
                    break;
                case MouseButton.WheelDown:
                    if(camera.Zoom.X > 1)
                        camera.Zoom -= Vector2.One * scaleSpeed;
                    break;
            }
        }
        else if (inputEvent is InputEventMouseMotion mm && dragging)
        {
            Vector2 delta = mm.Position - lastMousePos;
            Position -= delta / camera.Zoom;

            lastMousePos = mm.Position;
        }
    }
}