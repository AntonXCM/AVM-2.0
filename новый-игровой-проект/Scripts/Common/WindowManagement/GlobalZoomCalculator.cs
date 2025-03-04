using Godot;
using System;

public partial class GlobalZoomCalculator : Node
{
	public static float Zoom = 3;
	public static Vector2 ZoomVector;
	public static event Action OnZoomChanged;
	[Export] float targetHeight, pixelSize;
	Window window;
	public override void _Ready()
	{
		window = GetTree().Root;
		window.SizeChanged += UpdateZoom;
		UpdateZoom();
	}

	void UpdateZoom()
	{
		Zoom = window.Size.Y / targetHeight * pixelSize;
		float leftover =Zoom % (1 / pixelSize);
		if(Zoom != leftover) 
			Zoom -= leftover;
		ZoomVector = new(Zoom, Zoom);
		OnZoomChanged?.Invoke();
	}
}
