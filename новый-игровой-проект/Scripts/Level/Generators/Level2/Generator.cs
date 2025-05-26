using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public partial class Generator : Node
{
    [Export] Rect2I roomSize;
    RandomPickFactory<ISubdivisor> subdivisorFactory;

    public Generator()
    {
        subdivisorFactory = new([new TiledSubdivisor()]);
    }
    public void Generate(TileMapLayer debug, TileMapLayer gameplay)
    {
        debug.Clear();

        ISubdivisor subdivisor = subdivisorFactory.Get();
        var colorEnum = GetColors();

        LevelSubdivision subdivision = subdivisor.Subdivide(roomSize);

        var end = FindEnd(subdivision);
        var path = FindPath(FindStart(subdivision),r => r == end);
        if (path.Count == 0) return;
        foreach (var rect in subdivision)
            DebugDrawRegion(rect, debug, colorEnum);
        while (path.Count > 1)
        {
            LevelRegion current = path.Pop();
            DebugDrawRegionBounds(current, debug, colorEnum);
            current.RequireTransition(path.Peek());
            path.Peek().RequireTransition(current);
        }
        DebugDrawRegionBounds(path.Pop(), debug, colorEnum);
        IEnumerator<Vector2I> GetColors()
        {
        Start:
            yield return new(0, 0);
            yield return new(1, 0);
            yield return new(2, 0);
            yield return new(0, 1);
            yield return new(1, 1);
            yield return new(2, 1);
            yield return new(0, 2);
            yield return new(1, 2);
            yield return new(2, 2);
            goto Start;
        }
    }
    private static void DebugDrawRegionBounds(LevelRegion rect, TileMapLayer tileMap, IEnumerator<Vector2I> colorEnum)
    {
        colorEnum.MoveNext();
        Vector2I tileSetCoords = colorEnum.Current;
        for (int x = rect.bounds.Position.X; x < rect.bounds.End.X; x++)
        {
            tileMap.SetCell(new(x, rect.bounds.Position.Y), 0, tileSetCoords);
            tileMap.SetCell(new(x, rect.bounds.End.Y - 1), 0, tileSetCoords);
        }
        for (int y = rect.bounds.Position.Y + 1; y < rect.bounds.End.Y-1; y++)
        {   
            tileMap.SetCell(new(rect.bounds.Position.X, y),0, tileSetCoords);
            tileMap.SetCell(new(rect.bounds.End.X - 1, y),0, tileSetCoords);
        }
    }
    private static void DebugDrawRegion(LevelRegion rect, TileMapLayer tileMap, IEnumerator<Vector2I> colorEnum)
    {
        colorEnum.MoveNext();
        Vector2I tileSetCoords = colorEnum.Current;
        foreach (var pos in rect)
            tileMap.SetCell(pos, 0, tileSetCoords);
    }
    #region Pathfinding
    public static LevelRegion FindEnd(LevelSubdivision level)
    {
        LevelRegion end = level.First();

        foreach (var region in level)
        {
            if (region.bounds.End.X > end.bounds.End.X ||
               (region.bounds.End.X == end.bounds.End.X &&
                region.bounds.Position.Y > end.bounds.Position.Y))
                end = region;
        }

        return end;
    }
    public static LevelRegion FindStart(LevelSubdivision level)
    {
        LevelRegion start = level.First();

        foreach (var region in level)
        {
            if (region.bounds.Position.X < start.bounds.Position.X ||
               (region.bounds.Position.X == start.bounds.Position.X &&
                region.bounds.Position.Y > start.bounds.Position.Y))
                start = region;
        }

        return start;
    }
    public static Stack<LevelRegion> FindPath(LevelRegion start, Predicate<LevelRegion> endPredicate)
    {
        HashSet<LevelRegion> visited = new HashSet<LevelRegion>();
        Stack<LevelRegion> stack = new Stack<LevelRegion>();

        stack.Push(start);
        visited.Add(start);

        while (stack.Count > 0)
        {
            LevelRegion current = stack.Peek();

            if (endPredicate(current))
                break;

            var neighbors = current.Neighbors().Where(n => !visited.Contains(n)).ToArray();

            if (neighbors.Length > 0)
            {
                LevelRegion next = neighbors[Random.Shared.Next(neighbors.Length)];
                stack.Push(next);
                visited.Add(next);
            }
            else
                stack.Pop(); // Откат назад
        }

        return stack;
    }
    #endregion
}