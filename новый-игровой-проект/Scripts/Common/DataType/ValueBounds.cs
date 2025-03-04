using System;

[Serializable]
public struct ValueBounds
{
	public float Min, Max;

	public ValueBounds(float min, float max)
	{
		Max = max;
		Min = min;
	}
}

public static class ValueBoundsExtentions
{
	public static float Clamp(this ValueBounds bounds, float Float) => Math.Clamp(Float, bounds.Min, bounds.Max);
	public static bool IsInBounds(this ValueBounds bounds, float Float) => Float <= bounds.Max && Float >= bounds.Min;

	public static bool IsInBounds(this ValueBounds bounds, float Float, bool countEquality) =>
		countEquality ? IsInBounds(bounds, Float) : Float < bounds.Max && Float > bounds.Min;
}
