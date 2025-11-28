using Godot;
using System.Threading.Tasks;
public partial class RechargingEnergy : Node, IEnergy
{
    [Export] int max;
    [Export] float rechargeTime;
    RechargingValue valueRecharger;

    public override void _EnterTree()
    {
        foreach (var node in GetParent().GetChildrenRecursive())
            if (node is IRequireEnergy energy)
                energy.Energy = this;
    }
    public override void _Ready()
    {
        value = max;
        valueRecharger = new(new(0, max), ref value, rechargeTime, 1f, t => Task.Delay((int)(t * 1000)));
        valueRecharger.ValueChanged += Changed;
    }
    private float value;
    public int Max { get => max; set => max = value; }
    public int Value { get => valueRecharger; set => valueRecharger.Value = value; }
    void Changed(float oldValue, float newValue) => OnChanged?.Invoke((int)oldValue, (int)newValue);
    public event ChangeNotificator<int> OnChanged;
}