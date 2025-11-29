using System;
using Godot;

public partial class ZoomCalculator : Node
{
	public float Zoom = 3;
	public Vector2 ZoomVector = new(3,3);
	public Vector2 InverseZoomVector = new(1f/3f,1f/3f);
	public event Action OnZoomChanged;
	[Export] float targetHeight, pixelSize;
	Viewport viewport;
	public override void _Ready()
	{
		viewport = this.TryGetGrandparent<SubViewport>(out var parentViewport) ? parentViewport : GetWindow();
		viewport.SizeChanged += UpdateZoom;
		UpdateZoom();
	}

	void UpdateZoom()
	{
		Zoom = ((Vector2)viewport.Get("size")).Y / targetHeight * pixelSize;
		float leftover = Zoom % 1;
		if (Zoom != leftover)
			Zoom = Mathf.Max(Zoom - leftover, 1 / 3f);
		ZoomVector = new(Zoom, Zoom);
		InverseZoomVector = ZoomVector.Inverse();
		OnZoomChanged?.Invoke();
	}
}
