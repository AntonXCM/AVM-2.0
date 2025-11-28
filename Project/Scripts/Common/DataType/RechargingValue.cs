using System;
using System.Threading.Tasks;

public class RechargingValue
{
	public event ChangeNotificator<float> ValueChanged;
	public ValueBounds Bounds;
	private float val;
	private bool isOnReload;

	public float Value
	{
		get { return val; }
		set
		{
			float oldValue = val;
			val = Bounds.Clamp(value);
			ValueChanged?.Invoke(oldValue,val);
			_ = Reload();
		}
	}

	public float RechargeStep = 1, RechargeTime = 1;

	public delegate Task ReloadInstruction(float time);
	protected ReloadInstruction reloadDelegate;
	public RechargingValue(ValueBounds bounds,ref float value,float rechargeTime,float rechargeStep,ReloadInstruction reload)
	{
		Bounds = bounds;
		val = value;
		RechargeTime = rechargeTime;
		RechargeStep = rechargeStep;
		reloadDelegate = reload;
	}

	private async Task Reload()
	{
		if(!isOnReload && Bounds.IsInBounds(Value) && RechargeStep > 0)
		{
			isOnReload = true;
			while(Bounds.IsInBounds(Value + RechargeStep))
			{
				await reloadDelegate(RechargeTime);
				Value += RechargeStep;
			}
			isOnReload = false;
		}
	}

	#region operators
	public static implicit operator int(RechargingValue value) => (int)value.Value;
	public static implicit operator float(RechargingValue value) => value.Value;
	public static RechargingValue operator ++(RechargingValue value)
	{
		value.Value += 1;
		return value;
	}

	public static RechargingValue operator --(RechargingValue value)
	{
		value.Value -= 1;
		return value;
	}

	public static RechargingValue operator +(RechargingValue value,float increment)
	{
		value.Value += increment;
		return value;
	}

	public static RechargingValue operator -(RechargingValue value,float decrement)
	{
		value.Value -= decrement;
		return value;
	}

	public static RechargingValue operator *(RechargingValue value,float multiplier)
	{
		value.Value *= multiplier;
		return value;
	}

	public static RechargingValue operator /(RechargingValue value,float divisor)
	{
		value.Value /= divisor;
		return value;
	}

	public override string ToString() => Value.ToString();
	#endregion
}