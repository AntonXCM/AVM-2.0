using Godot;
using System;

public partial class ComplexGenerable : Node2D, IGenerable
{
    public void Generate()
    {
        foreach (var child in this.GetChildrenRecursive())
            if (child is IGenerable generable)
                generable.Generate();
    }
}
