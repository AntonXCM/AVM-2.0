using Godot;

namespace DustyStudios.MathAVM
{
	public static class Vector3Extentions
	{
		public static Vector3I Round(this Vector3 vector, RoundMode mode)
		{
			return new Vector3I(
				RoundedValue(vector.X),
				RoundedValue(vector.Y),
				RoundedValue(vector.Z)
			);
			int RoundedValue(float value)
			{
				switch(mode)
				{
					case RoundMode.min: return (int)value;
					default: return Mathf.RoundToInt(value);
				}
			}
		}
		public static Vector3 Clamp(this Vector3 vector, float min, float max) =>
			new(
				Mathf.Clamp(vector.X, min, max),
				Mathf.Clamp(vector.Y, min, max),
				Mathf.Clamp(vector.Z, min, max)
			);
		public static Vector3 NormalizedMin1(this Vector3 vector)
		{
			vector = vector.Normalized();
			if(vector != Vector3.Zero)
			{
				float minValue = Mathf.Abs(vector.X);
				SetToMinAndNot0(vector.Y);
				SetToMinAndNot0(vector.Z);
				void SetToMinAndNot0(float second)
				{
					if(Mathf.Abs(second) > minValue) minValue = second;
				}
				vector /= minValue;
			}
			return vector;
		}
	}
}