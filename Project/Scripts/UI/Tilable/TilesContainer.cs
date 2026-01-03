using System;
using Godot;

public partial class TilesContainer : Control
{
    [Export] public bool Horizontal = true, ScaleWithParent = false;
    float[] weights = [];
    ResizeHandle[] handles = [];
    private Control parent;
    public override void _Ready()
    {
        Name = "Tiles Container " + (char)(GD.Randi() % 27 + 'A');
        UpdateLayout();
    }
    public void Unpack()
    {
        GetChild(0).Reparent(parent);
        QueueFree();
    }
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        foreach (var handle in handles)
            handle.QueueFree();
        if (parent is TilesContainer container)
            container.UpdateLayout();
    }

    public override void _EnterTree()
    {
        parent = (Control)GetParent();
        if (ScaleWithParent)
            parent.Resized += () => Size = parent.Size;
    }
    public void AcceptChild(Node node)
    {
        if (node is not Control added) throw new Exception("Э! чё ты мне сунул?");
        var addedCenterPosition = added.GetCenterPosition();

        bool wantHorizontal = Horizontal;
        if (addedCenterPosition.X > Size.X * 0.75f || addedCenterPosition.X < Size.X * 0.25f)
            wantHorizontal = true;
        else if (addedCenterPosition.Y > Size.Y * 0.75f || addedCenterPosition.Y < Size.Y * 0.25f)
            wantHorizontal = false;
        
        if (GetChildCount() >= 3 && wantHorizontal != Horizontal)
        {
            Control neighbor = FindNearestNeighbor(added);
            Control FindNearestNeighbor(Control added)
            {
                float best = float.MaxValue;
                Control last = added;
                for (int i = GetChildCount() - 2; i >= 0; i--)
                {
                    Control c = (Control)GetChild(i);
                    if (c == added) continue;
                    float d = Horizontal ? Mathf.Abs(added.GetCenterPosition().X - c.GetCenterPosition().X)
                                         : Mathf.Abs(added.GetCenterPosition().Y - c.GetCenterPosition().Y);
                    if (d > best || i is 0)
                        return last;
                    last = c;
                    best = d;
                }
                throw new Exception("How?");
            }
            if (neighbor is not null)
            {
                var sub = new TilesContainer
                {
                    Horizontal = wantHorizontal,
                    Size = neighbor.Size,
                    Position = neighbor.Position
                };
                added.Reparent(sub);
                neighbor.Reparent(sub);
                AddChild(sub);
                AcceptChild(sub);
                return;
            }
        }
        Horizontal = wantHorizontal;
        for (int i = 0; i < GetChildCount() - 1; i++)
        {
            var childsCenterPosition = ((Control)GetChild(i)).GetCenterPosition();
            if (Horizontal)
            {
                if (childsCenterPosition.X < addedCenterPosition.X)
                    continue;
            }
            else if (childsCenterPosition.Y < addedCenterPosition.Y)
                continue;
            MoveChild(added, i);
            break;
        }

        UpdateLayout();
    }
    public override void _Notification(int what)
    {
        if (what == NotificationResized)
            UpdateLayout();
    }

    public void UpdateLayout()
    {
        int childCount = GetChildCount();
        if (weights.Length != childCount)
        {
            weights = new float[childCount];
            float weight = 1f / childCount;
            for (int i = 0; i < childCount; i++)
                weights[i] = weight;
        }
        
        int handlesCount = childCount - 1;
        if (handlesCount != handles.Length)
        {
            var newHandles = new ResizeHandle[handlesCount];
            if (handlesCount > handles.Length)
                for (int i = 0; i < handlesCount; i++)
                    if (i < handles.Length)
                        newHandles[i] = handles[i];
                    else
                    {
                        var h = new ResizeHandle();
                        Root.instance.CallDeferred("add_child", h);
                        newHandles[i] = h;
                    }
            else
                for (int i = 0; i < handles.Length; i++)
                    if (i < handlesCount)
                        newHandles[i] = handles[i];
                    else
                        handles[i].QueueFree();
            handles = newHandles;
        }

        var available = Size;
        float offset = 0;

        for (int i = 0; i < childCount; i++)
        {
            var c = (Control)GetChild(i);
            if (Horizontal)
            {
                float w = available.X * weights[i];
                c.Position = new Vector2(offset, 0);
                c.Size = new Vector2(w, available.Y);
                offset += w;
                if(i != handles.Length)
                    handles[i].Setup(this, i, true, new Rect2(GlobalPosition.X + offset - 4, GlobalPosition.Y, 8, available.Y));
            }
            else
            {
                float h = available.Y * weights[i];
                c.Position = new Vector2(0, offset);
                c.Size = new Vector2(available.X, h);
                offset += h;

                if(i != handles.Length)
                    handles[i].Setup(this, i, false, new Rect2(GlobalPosition.X, GlobalPosition.Y + offset - 4, available.X, 8));
            }
        }

    }
    public void AdjustWeight(int index, float delta)
    {
        if (index < 0 || index >= weights.Length - 1) return;
        weights[index] = Mathf.Clamp(weights[index] + delta, 0.05f, 0.95f);
        weights[index + 1] = Mathf.Clamp(weights[index + 1] - delta, 0.05f, 0.95f);
        UpdateLayout();
    }
}
