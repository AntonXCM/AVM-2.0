using Godot;
public partial class TilesContainer : Control
{
    [Export] public bool Horizontal = true, IsRoot = false;
    float[] weights = [];
    ResizeHandle[] handles = [];
    private Control parent;
    public override void _Ready()
    {
        if (IsRoot)
            Root.instance.RootContainer = this;
        else
            Root.instance.Containers.Add(this);

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
        Root.instance.Containers.Remove(this);
        foreach (var handle in handles)
            handle.QueueFree();
        if (parent is TilesContainer container)
            container.UpdateLayout();
    }

    public override void _EnterTree()
    {
        parent = (Control)GetParent();
        if (IsRoot)
            parent.Resized += () => Size = parent.Size;
    }
    public Control GetNearestNeighbor(Control to)
    {
        float best = float.MaxValue;
        Control last = to;
        for (int i = GetChildCount() - 1; i >= 0; i--)
        {
            Control c = (Control)GetChild(i);
            if (c == to) continue;
            float d = Horizontal ? Mathf.Abs(to.GetCenterPosition().X - c.GetCenterPosition().X)
                                 : Mathf.Abs(to.GetCenterPosition().Y - c.GetCenterPosition().Y);
            if (d > best)
                return last;
            last = c;
            best = d;
        }
        return last;
    }
    public void AcceptChild(Node node)
    {
        if (node is not Control added) throw new System.Exception("Э! чё ты мне сунул?");
        var addedCenterPosition = added.GetCenterPosition();

        bool wantHorizontal = Horizontal;
        if (addedCenterPosition.X > Size.X * 0.75f || addedCenterPosition.X < Size.X * 0.25f)
            wantHorizontal = true;
        else if (addedCenterPosition.Y > Size.Y * 0.75f || addedCenterPosition.Y < Size.Y * 0.25f)
            wantHorizontal = false;
        
        if (GetChildCount() >= 3 && wantHorizontal != Horizontal)
        {
            Control neighbor = GetNearestNeighbor(added);
            if (neighbor is not null)
            {
                var sub = new TilesContainer
                {
                    Horizontal = wantHorizontal,
                    Size = neighbor.Size,
                    Position = neighbor.Position
                };
                if (wantHorizontal)
                {
                    if (added.Position.X < neighbor.Position.X)
                    {
                        added.Reparent(sub);
                        neighbor.Reparent(sub);
                    }
                    else
                    {
                        neighbor.Reparent(sub);
                        added.Reparent(sub);
                    }
                }
                else
                {
                    if (added.Position.Y < neighbor.Position.Y)
                    {
                        added.Reparent(sub);
                        neighbor.Reparent(sub);
                    }
                    else
                    {
                        neighbor.Reparent(sub);
                        added.Reparent(sub);
                    }
                }
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
        if (!IsRoot)
        {
            if (childCount == 1)
            {
                Unpack();
                return;
            }
            else if (childCount == 0)
            {
                QueueFree();
                return;
            }
        }
        if (weights.Length != childCount)
        {
            weights = new float[childCount];
            float weight = 1f / childCount;
            for (int i = 0; i < childCount; i++)
                weights[i] = weight;
        }

        int handlesCount = Mathf.Max(0, childCount - 1);
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
                if (i < handles.Length)
                    handles[i].Setup(this, i, true, new Rect2(GlobalPosition.X + offset - 4, GlobalPosition.Y, 8, available.Y));
            }
            else
            {
                float h = available.Y * weights[i];
                c.Position = new Vector2(0, offset);
                c.Size = new Vector2(available.X, h);
                offset += h;

                if (i < handles.Length)
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
