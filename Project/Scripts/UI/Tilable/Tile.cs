using Godot;
public partial class Tile : Control
{
    bool dragging = false;
    [Export] TilesContainer parent;
    public override void _Ready()
    {
        TreeEntered += Parented;
        Parented();
    }

    public void Parented()
    {
        var newParent = GetParentOrNull<TilesContainer>();
        if (newParent is not null)
            parent = newParent;
    }
    public override void _Input(InputEvent e)
    {
        if(Root.instance.handledInput) return;
        if (e is InputEventMouseButton mb && mb.ButtonIndex is MouseButton.Left)
        {
            if (mb.Pressed && GetGlobalRect().HasPoint(mb.Position))
                StartDragging();
            else if (dragging)
                EndDragging();
        }
        else if (e is InputEventMouseMotion mm && dragging)
            Position += mm.Relative;
    }
    private void StartDragging()
    {
        dragging = true;
        Reparent(Root.instance);
    }
    private void EndDragging()
    {
        dragging = false;
        var pos = GetGlobalMousePosition();
        TilesContainer target = null;
        foreach (var ch in Root.instance.GetChildren())
            if (ch is TilesContainer g && g.GetGlobalRect().HasPoint(pos))
            {
                target = g;
                break;
            }
        if ((target is null || parent == target) && parent != null)
            target = parent;
        else if (parent.GetChildCount() == 0)
            parent.QueueFree();
        GD.Print("Parented", parent.Name);
        Reparent(parent);
        parent.AcceptChild(this);
        target.UpdateLayout();
        parent = target;
    }
}