using DustyStudios.MathAVM;

using Godot;

using System.Collections.Generic;
using System.Linq;

public partial class Chain : Node2D, IHasVelocity
{
	[Export] public PackedScene LinkPrefab; 
	[Export] public int MaxCapacity = 10; 
	[Export] public Vector2 Speed, Spacing;

	private Queue<Node2D> links = new Queue<Node2D>();
	private Vector2 spawn;

	public Vector2 Velocity => Speed;

	public override void _Ready()
	{
		spawn = Spacing*(MaxCapacity-1);
		for(int i = 0; i < MaxCapacity; i++)
			AddLink();
	}

	public override void _Process(double delta)
	{
		MoveChain(Speed * (float)delta);
	}

	private void MoveChain(Vector2 offset)
	{
		float minLength = links.Peek().Position.LengthSquared();
		foreach(var link in links)
		{
			link.Position += offset;
			minLength = Mathf.Min(link.Position.LengthSquared(),minLength);
		}
		if(minLength > Spacing.LengthSquared())
		{
			GD.Print("worked");
			var link = links.Dequeue();
			link.Position = links.Last().Position - Spacing;
			links.Enqueue(link);
		}
	}

	private void AddLink()
	{
		var link = (Node2D)LinkPrefab.Instantiate();
		link.Position = spawn;
		spawn -= Spacing;
		AddChild(link);
		links.Enqueue(link);
		if(links.Count > MaxCapacity) links.Dequeue().QueueFree();
	}
}