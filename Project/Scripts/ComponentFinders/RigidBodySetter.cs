using Godot;
using Godot.Collections;

public partial class RigidBodySetter : Node
{
	public override void _Ready()
	{
		var rb = GetParent<CharacterBody2D>();
		ProcessChildren(rb.GetChildren());
		void ProcessChildren(Array<Node> nodes)
		{
			foreach(var node in nodes)
			{
				if(node is IHasRigidBody2D)
					(node as IHasRigidBody2D).Rb = rb;
				ProcessChildren(node.GetChildren());
			}
			return;
		}
	}
}
