using Godot;
using Godot.Collections;
public partial class RandomChildInstantitator : Node, IGenerable
{
    [Export] public Array<PackedScene> PossibleChildren;
    public override void _Ready() => Generate();
    public void Generate()
    {
        foreach (var child in GetChildren())
        {
            RemoveChild(child);
            child.QueueFree();
        }
        var background = PossibleChildren.PickRandom().Instantiate();
        AddChild(background);
        if (background is IGenerable generable)
            generable.Generate();
    }
}