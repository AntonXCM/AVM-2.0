using Godot;
using System;
using System.Collections.Generic;
public static class NodeExtentions
{
	public static bool TryGetGrandparent<T>(this Node node, out T grandparent) where T : class
	{
		if (node is null)
		{
			grandparent = null;
			return false;
		}
		if (node is T result)
		{
			grandparent = result;
			return true;
		}
		else return node.GetParent().TryGetGrandparent(out grandparent);
	}
	public static bool TryGetGrandchild<T>(this Node node, out T grandchild) where T : class
	{
		foreach (Node child in node.GetChildrenRecursive())
			if (child is T resulT)
			{
				grandchild = resulT;
				return true;
			}
		grandchild = null;
		return false;
	}
	public static T GetGrandparent<T>(this Node node) where T : class
	{
		if (node is null)
			throw new KeyNotFoundException($"Не нашёл дедв типа {typeof(T).Name} у *смотри стек вызовов* 🖕");
		if(node is T result)
			return result;
		else return node.GetParent().GetGrandparent<T>();
	}
	public static T GetGrandchild<T>(this Node node) where T : class
	{
		foreach (Node child in node.GetChildrenRecursive())
			if (child is T resulT)
				return resulT;
		throw new KeyNotFoundException($"Не нашёл внука типа {typeof(T).Name} у {node.Name} 🖕");
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
		Dictionary<string, Node> dict = [];

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