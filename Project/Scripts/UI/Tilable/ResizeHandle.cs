using Godot;
public partial class ResizeHandle : Control
{
    TilesContainer group;
    int index;
    bool horizontal, dragging;
    public void Setup(TilesContainer container, int i, bool horizontal, Rect2 rect)
    {
        group = container;
        index = i;
        this.horizontal = horizontal;
        Position = rect.Position;
        Size = rect.Size;
        MouseFilter = MouseFilterEnum.Stop;
    }
    public override void _Input(InputEvent e)
    {
        if (e is InputEventMouseButton mb && mb.ButtonIndex == MouseButton.Left)
        {
            if (mb.Pressed && GetGlobalRect().HasPoint(mb.Position))
                dragging = true;
            else
                dragging = false;
        }
        else if (dragging && e is InputEventMouseMotion mm)
        {
            float d = horizontal ? mm.Relative.X / group.Size.X : mm.Relative.Y / group.Size.Y;
            group.AdjustWeight(index, d);
            Root.instance.handledInput = true;
        } 
    }

    public override void _Draw() => DrawRect(new Rect2(Vector2.Zero, Size), new Color(0, 0, 0, 1)); //Заглушка
}
