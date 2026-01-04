using System.Collections.Generic;
using Godot;
public partial class Root : Control
{
    public static Root instance;
    public HashSet<TilesContainer> Containers = [];
    public TilesContainer RootContainer;
    [ExportGroup("Style")]
    [Export] public Color ResizeHandleColor = Colors.White, TargetColor = Colors.DodgerBlue;
    [Export] public Vector2 DragSize = new(1.1f, 1.1f);
    public bool handledInput = false;
    public Root() => instance = this;
    public override void _Ready()
    {
        Resize();
        GetViewport().SizeChanged += Resize;
    }
    public override void _Input(InputEvent @event) => handledInput = false;
    void Resize() => Size = GetViewportRect().Size;

    private Control currentDragger;
    public bool StartDrag(Control dragger)
    {
        if (currentDragger is not null && dragger.ZIndex <= currentDragger.ZIndex)
            return false;
        currentDragger = dragger;
        return true;
    }
    public void EndDrag(Control owner)
    {
        if (owner == currentDragger)
            currentDragger = null;
    }
}