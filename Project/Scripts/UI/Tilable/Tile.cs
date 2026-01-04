using Godot;
public partial class Tile : Control
{
    TilesContainer target;
    [Export] bool dragging;
    public bool Dragging
    {
        get => dragging;
        set
        {
            if (dragging == value) return;
            ZIndex = value ? 10 : 0;
            Scale = value ? Root.instance.DragSize : Vector2.One;
            //TODO: Add shadow    
            dragging = value;
        }
    }
    public override void _Input(InputEvent e)
    {
        if (e is InputEventMouseButton mb && mb.ButtonIndex is MouseButton.Left)
        {
            if (mb.Pressed && GetGlobalRect().HasPoint(mb.Position) && Root.instance.StartDrag(this))
            {
                Dragging = true;
                var parent = (TilesContainer)GetParent();
                Reparent(Root.instance);
                parent.UpdateLayout();
            }
            else if (Dragging)
            {
                Dragging = false;
                Root.instance.EndDrag(this);
                if (!IsInstanceValid(target) || !target.IsInsideTree()) return;
                Reparent(target);
                target.AcceptChild(this);
            }
        }
        if (e is not InputEventMouseMotion mm || !Dragging) return;
        Position += mm.Relative;
        FindTarget();
        QueueRedraw();
    }
    private void FindTarget()
    {
        var pos = GetGlobalMousePosition();
        foreach (var ch in Root.instance.Containers) //Отсюда контейнеры удаляются в Dispose
            if (ch.GetGlobalRect().HasPoint(pos))
            {
                target = ch;
                return;
            }
        target = Root.instance.RootContainer;
    }
    public override void _Draw()
    {
        if (!Dragging || !IsInstanceValid(target) || !target.IsInsideTree()) return;
        Control control = target.GetNearestNeighbor(this);
        Rect2 rect = GetGlobalTransform().AffineInverse() * control.GetGlobalRect();
        DrawRect(rect, Root.instance.TargetColor);
    }
}