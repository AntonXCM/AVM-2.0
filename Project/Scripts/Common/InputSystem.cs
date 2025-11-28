using Godot;
using System;
using System.Collections.Generic;

public partial class InputSystem : Node
{
    public static AccelerationControlType AccelerationControl = AccelerationControlType.ShiftCtrl;
    public enum AccelerationControlType
    {
        WandD,
        ShiftCtrl
    }
    public static event Action OnInput;
    static readonly HashSet<string> keys = [];
    public override void _Input(InputEvent Event)
    {
        if (Event is InputEventKey eventKey)
        {
            string key = OS.GetKeycodeString(eventKey.Keycode);
            if (Event.IsPressed())
            {
                if (keys.Contains(key))
                    return;
                keys.Add(key);
            }
            else
                keys.Remove(key);

            OnInput?.Invoke();
        }
        //Пока что игнорируем другие инпуты.
    }
    public static bool IsPressed(string name) => keys.Contains(name);
    public static bool HasHoryzontal() => keys.Contains("A") ^ keys.Contains("D");
    public static Vector2 GetWASD() =>  new(
        (keys.Contains("D") ? 1 : 0) - (keys.Contains("A") ? 1 : 0),
        (keys.Contains("S") ? 1 : 0) - (keys.Contains("W") ? 1 : 0))
        ;
	public static Vector2 GetVelocity()
    {
        Vector2 v = GetWASD();
        return v * v.Length();
    }

    public static Vector2 GetHoryzontalVelocity(float speed = 1) => new Vector2(((keys.Contains("D") ? 1 : 0) - (keys.Contains("A") ? 1 : 0))
        * AccelerationControl switch
        {
            AccelerationControlType.WandD => keys.Contains("W") || keys.Contains("S") ? speed * 2 : speed,
            AccelerationControlType.ShiftCtrl => keys.Contains("Shift") || keys.Contains("Ctrl") ? speed * 2 : speed,
            _ => speed
        }, 0);

    public static HashSet<string> GetKeys() => [.. keys];
}
