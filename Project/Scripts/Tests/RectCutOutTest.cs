using DustyStudios.MathAVM;
using Godot;

namespace Tests;
public static partial class Tests
{
    public static void CutByTest()
    {
        Rect2I rect = new(Vector2I.Zero, Vector2I.One * 4);
        rect.CutBy(new Rect2I(Vector2I.One, Vector2I.One * 5));
        GD.Print(rect);
    }
}