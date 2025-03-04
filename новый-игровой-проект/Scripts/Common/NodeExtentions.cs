using Godot;
using System;
using System.Collections.Generic;
public static class NodeExtentions
{
	public static bool TryGetComponentInParent<T>(this Node node, out T component)
	{
		if (node == null)
		{
			component = default;
			return false;
		}
		if (node is T result)
		{
			component = result;
			return true;
		}
		else return node.GetParent().TryGetComponentInParent(out component);
	}

	public static IEnumerable<Node> GetChildrenRecursive(this Node node)
	{
		for (int i = node.GetChildCount() - 1; i >= 0;)
		{
			Node child = node.GetChild(i--);
			yield return child;

			foreach (Node descendant in child.GetChildrenRecursive())
				yield return descendant;
		}
	}
	public static Dictionary<string, Node> GetChildrenDictRecursive(this Node node)
	{
		Dictionary<string, Node> dict = new Dictionary<string, Node>();

		foreach (Node child in node.GetChildrenRecursive())
			if (!dict.ContainsKey(child.Name))
				dict.Add(child.Name, child);

		return dict;
	}
	public static T AddChild<T>(this Node node, Func<T> createChild) where T : Node
    {
        T child = createChild();
     	node.AddChild(child);
        return child;
    }
}