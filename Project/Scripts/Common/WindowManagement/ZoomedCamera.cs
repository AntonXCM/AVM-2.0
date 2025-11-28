using Godot;

public partial class ZoomedCamera : Camera2D
{
	public override void _Ready()
	{
		ZoomCalculator calculator = GetNode<ZoomCalculator>("Zoom calculator");
		Zoom = calculator.ZoomVector;
		calculator.OnZoomChanged += () => Zoom = calculator.ZoomVector;
	}
}