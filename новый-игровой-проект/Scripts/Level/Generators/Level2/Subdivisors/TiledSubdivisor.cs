using System;
using DustyStudios.MathAVM;
using Godot;

public class TiledSubdivisor : ISubdivisor
{
    public LevelSubdivision Subdivide(Rect2I rect)
    {
        GDScript resource = (GDScript)GD.Load("res://Scripts/Level/Generators/Level2/Subdivisors/TiledSubdivisor.gd");
        return resource.Call("subdivide", rect).Obj as LevelSubdivision;
        LevelSubdivision result = new();

        foreach (var part in rect.SplitHorizontally((ushort)Random.Shared.Next(3, 7)))
            foreach (var partPart in part.SplitVertically((ushort)Random.Shared.Next(1, 4)))
                result.Add(partPart);
        foreach (var region in result)
        {
            switch (Random.Shared.NextDouble())
            {
                case double a when a < 0.2:
                    if (region.bounds.Position.Y > rect.Position.Y)
                        region.ExpandUp(1);
                    break;
                case double a when a > 0.2 && a < 0.4:
                    if (region.bounds.End.Y < rect.End.Y)
                        region.ExpandDown(1);
                    break;
                case double a when a > 0.9:
                    if (region.bounds.Position.X > rect.Position.X)
                        region.ExpandLeft(1);
                    break;
                case double a when a > 0.8 && a < 0.9:
                    if (region.bounds.End.X < rect.End.X)
                        region.ExpandRight(1);
                    break;
            }
        }
        return result;
    }
}