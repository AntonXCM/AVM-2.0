using System;
using Godot;
public partial class RandomChildInstantitator : Node, IGenerable
{
    [Export] public PackedScene[] PossibleChildren;
    public override void _Ready() => Generate();
    public void Generate()
    {
        foreach (var child in GetChildren()) child.QueueFree();
        AddChild(PossibleChildren[Random.Shared.Next(PossibleChildren.Length)].Instantiate());
    }
}
