using Godot;
public partial class Root : Control
{
    public static Root instance;
    public bool handledInput = false;
    public Root() => instance = this;
    public override void _Ready()
    {
        ResizeAll();
        GetViewport().SizeChanged += ResizeAll;
    }
    public override void _Input(InputEvent @event) => handledInput = false;
    void ResizeAll() => Size = GetViewportRect().Size;
}