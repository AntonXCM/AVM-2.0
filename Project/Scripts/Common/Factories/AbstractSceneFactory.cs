namespace DustyStudios.AVM2.Factories;
using Godot;
using System;
using System.Collections.Generic;

public partial class AbstractSceneFactory : Node
{
    [Export] public PackedScene SceneProduct;

    public Node Create(Dictionary<string, Func<Node>> additions = null)
    {
        Node instance = SceneProduct.Instantiate();

        if (additions != null)
            instance.GetChildrenDictRecursive().ProcessOverlapps(additions, (c, f) => c.AddChild(f));

        AddChild(instance);
        return instance;
    }
}