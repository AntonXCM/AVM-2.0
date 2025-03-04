using Godot;
namespace DustyStudios.AVM2.UI;

public partial class StatIndicator : Node
{
	public IIntegerStat Stat;
	private Node[] indicators;

	public StatIndicator(){}
	public StatIndicator(IIntegerStat stat, PackedScene indicatorScene)
	{
		Name = "Indication Controller";
		indicators = new Node[stat.Max];

		for (int i = 0; i < stat.Max; i++)
		{
			var indicator = indicatorScene.Instantiate();
			indicators[i] = indicator;
			AddChild(indicator);
			indicator.CallDeferred("Enable");
		}
		stat.OnChanged += (o, n) => UpdateIndicators(o,n);
		Stat = stat;
	}
	public override void _Notification(int what)
	{
		switch (what)
		{
			case (int)NotificationParented:
				var parent = GetParent();
				if (parent == null) return;
				foreach (var child in indicators)
					child.Reparent(parent);
				break;
		} 
    }
	private void UpdateIndicators(float oldValue, float newValue)
	{
		GD.Print($"oldValue: {oldValue}; newValue: {newValue}");
		if (newValue > oldValue)
			for (int i = Mathf.Max((int)oldValue, 0); i < newValue && i < Stat.Max; i++)
				indicators[i].Call("Enable");
		else
			for (int i = Mathf.Max((int)newValue, 0); i < oldValue && i < Stat.Max; i++)
				indicators[i].Call("Disable");
	}
}