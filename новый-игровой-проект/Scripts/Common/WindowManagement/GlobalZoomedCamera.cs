using Godot;

public partial class GlobalZoomedCamera : Camera2D
{
	public override void _Ready()
	{
		base._Ready();
		Zoom = GlobalZoomCalculator.ZoomVector;
		GlobalZoomCalculator.OnZoomChanged += () => Zoom = GlobalZoomCalculator.ZoomVector;
	}
}