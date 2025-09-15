using Godot;
using System.Collections.Generic;
using System.Linq;
namespace DustyStudios.MathAVM;

public enum TouchSide : byte { None, Left, Right, Top, Bottom }
[GodotClassName("Rect2Extentions")]
public static class Rect2Extentions
{
    #region CutBy
    public static Rect2I CutBy(this Rect2I rect, Rect2I avoid)
    {
        if (!rect.Intersects(avoid)) return rect;
        var options = GetOptions();
        IEnumerable<Rect2I> GetOptions()
        {
            yield return new Rect2I(rect.Position, new Vector2I(rect.Size.X, avoid.Position.Y - rect.Position.Y)); // Верхний обрезок
            yield return new Rect2I(new Vector2I(rect.Position.X, avoid.End.Y), new Vector2I(rect.Size.X, rect.End.Y - avoid.End.Y)); // Нижний обрезок
            yield return new Rect2I(rect.Position, new Vector2I(avoid.Position.X - rect.Position.X, rect.Size.Y)); // Левый обрезок
            yield return new Rect2I(new Vector2I(avoid.End.X, rect.Position.Y), new Vector2I(rect.End.X - avoid.End.X, rect.Size.Y)); // Правый обрезок
        }
        options = options.Where(r => r.Size.X > 0 && r.Size.Y > 0);
        if (options.Count() == 0) return new();
        Rect2I best = options.OrderBy(r => r.Size.X * r.Size.Y).First();
        return best;
    }
    #endregion

    #region enumeration
    public static IEnumerator<Vector2I> GetEnumerator(this Rect2I rect)
    {
        for (Vector2I pos = rect.Position; pos.X < rect.End.X; pos.X++)
        {
            for (; pos.Y < rect.End.Y; pos.Y++)
                yield return pos;
            pos.Y = rect.Position.Y;
        }
    }
    #endregion

    #region split functions
    public static IEnumerable<Rect2I> SplitVertically(this Rect2I rect, ushort count)
    {
        int height = rect.Size.Y / count, remainder = rect.Size.Y % count, y = rect.Position.Y;

        for (int i = 0; i < count; i++)
        {
            int extra = (i < remainder) ? 1 : 0;
            yield return new Rect2I(rect.Position.X, y, rect.Size.X, height + extra);
            y += height + extra;
        }
    }
    public static IEnumerable<Rect2I> SplitHorizontally(this Rect2I rect, ushort count)
    {
        int width = rect.Size.X / count, remainder = rect.Size.X % count, x = rect.Position.X;

        for (int i = 0; i < count; i++)
        {
            int extra = (i < remainder) ? 1 : 0;
            yield return new Rect2I(new Vector2I(x, rect.Position.Y), new Vector2I(width + extra, rect.Size.Y));
            x += width + extra;
        }
    }
    #endregion

    public static TouchSide GetTouchSide(this Rect2I a, Rect2I b)
    {
        if (a.Position.Y < b.End.Y && a.End.Y > b.Position.Y)
        {
            if (a.End.X == b.Position.X)
                return TouchSide.Right;
            if (a.Position.X == b.End.X)
                return TouchSide.Left;
        }
        if (a.Position.X < b.End.X && a.End.X > b.Position.X)
        {    
            if (a.End.Y == b.Position.Y)
                return TouchSide.Bottom;
            if (a.Position.Y == b.End.Y)
                return TouchSide.Top;
        }
        return TouchSide.None;
    }
}