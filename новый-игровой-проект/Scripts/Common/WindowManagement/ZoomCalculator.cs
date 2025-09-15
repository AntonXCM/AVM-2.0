using System;
using Godot;

public partial class ZoomCalculator : Node
{
	public float Zoom = 3;
	public Vector2 ZoomVector = new(3,3);
	public event Action OnZoomChanged;
	[Export] float targetHeight, pixelSize;
	SubViewport viewport;
	public override void _Ready()
	{
		viewport = this.GetComponentInParents<SubViewport>();
		viewport.SizeChanged += UpdateZoom;
		UpdateZoom();
	}

	void UpdateZoom()
	{
		Zoom = viewport.Size.Y / targetHeight * pixelSize;
		float leftover = Zoom % 1;
		if (Zoom != leftover)
			Zoom = Mathf.Max(Zoom - leftover, 1 / 3f);
		ZoomVector = new(Zoom, Zoom);
		OnZoomChanged?.Invoke();
	}
}
