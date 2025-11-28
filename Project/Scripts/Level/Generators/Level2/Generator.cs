using DustyStudios.MathAVM;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public partial class Generator : Node
{
    [Export] Rect2I roomSize;
    [Export] Resource[] subdivisors, shapeCreators;
    [Export] Resource tileDrawer;
    [Export] ShaderMaterial generationMaterial;
    RandomPickFactory<Resource> subdivisorFactory, shapeCreatorFactory;
    
    private void Init()
    {
        subdivisorFactory = new([.. subdivisors]);
        shapeCreatorFactory = new([.. shapeCreators]);
    }
    public void Generate(TileMapLayer debug, TileMapLayer gameplay)
    {
        debug.Clear();
        gameplay.Clear();
        if (subdivisorFactory is null) Init();
        Resource subdivisor = subdivisorFactory.Get();
        var colorEnum = GetColors();

        LevelSubdivision subdivision = subdivisor.Call("subdivide", [roomSize]).Obj as LevelSubdivision;

        LevelRegion
            start = FindStart(subdivision),
            end = FindEnd(subdivision);
        var path = FindPath(start, r => r == end, subdivision);

        path.Peek().IsEnd = true;
        while (path.Count > 1)
        {
            LevelRegion current = path.Pop();

            current.RequireTransition(path.Peek());
            path.Peek().RequireTransition(current);

            DrawRectOutline(current.Bounds, debug, colorEnum);
        }
        path.Peek().IsStart = true;
        DrawRectOutline(path.Pop().Bounds, debug, colorEnum);

        Godot.Collections.Array<Godot.Collections.Array<int>> tilemap = [];
        for (int x = 0; x < roomSize.Size.X; x++)
        {
            tilemap.Add([]);
            for (int y = 0; y < roomSize.Size.Y; y++)
                tilemap[x].Add(0);
        }

        foreach (var region in subdivision)
        {
            var tiles = shapeCreatorFactory.Get().Call("create", region).AsGodotArray<Variant>();
            int xSize = Mathf.Min(region.Bounds.Size.X, tiles.Count);
            for (int x = 0; x < xSize; x++)
            {
                var column = tiles[x].AsGodotArray<int>();
                int ySize = Mathf.Min(region.Bounds.Size.Y, column.Count);
                for (int y = 0; y < ySize; y++)
                    tilemap[x + region.Bounds.Position.X][y + region.Bounds.Position.Y] = column[y];
            }
        }
        gameplay.Material = generationMaterial;
        string callbackID = GetNiceID.Current;
        GetNiceID.MoveNext();
        tileDrawer.Call("draw", tilemap, gameplay, callbackID);
        var changeMaterialBack = Callable.From<string>(ChangeMaterialBack);
        var showProcess = Callable.From<string, float>(ShowProcess);
        tileDrawer.Connect("draw_finished", changeMaterialBack);
        tileDrawer.Connect("draw_process", showProcess);
        void ChangeMaterialBack(string id)
        {
            if (id != callbackID) return;
            gameplay.Material = null;
            tileDrawer.Disconnect("draw_finished",changeMaterialBack);
            tileDrawer.Disconnect("draw_process",showProcess);
        }
        void ShowProcess(string id, float percentage)
        {
            if (id != callbackID) return;
            generationMaterial.SetShaderParameter("process", 1-percentage);
        }

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
    static IEnumerator<string> GetNiceID = EnumNiceID();
    static IEnumerator<string> EnumNiceID()
    {
    Start:
        yield return "Gena";
        yield return "Krokodil";
        yield return "Evgen";
        yield return "Jena";
        yield return "*3Ha";
        yield return "Pulukarov";
        yield return "Cheburashka";
        yield return "Genshtab";
        yield return "Cobblestone";
        yield return "Generator";
        yield return "Genius";
        yield return "Enshtain";
        yield return "Gento";
        yield return "Linux";
        yield return "GMO";
        goto Start;
    }
    private static void DrawRectOutline(Rect2I rect, TileMapLayer tileMap, IEnumerator<Vector2I> colorEnum)
    {
        colorEnum.MoveNext();
        Vector2I tileSetCoords = colorEnum.Current;
        for (int x = rect.Position.X; x < rect.End.X; x++)
        {
            tileMap.SetCell(new(x, rect.Position.Y), 0, tileSetCoords);
            tileMap.SetCell(new(x, rect.End.Y - 1), 0, tileSetCoords);
        }
        for (int y = rect.Position.Y + 1; y < rect.End.Y-1; y++)
        {   
            tileMap.SetCell(new(rect.Position.X, y),0, tileSetCoords);
            tileMap.SetCell(new(rect.End.X - 1, y),0, tileSetCoords);
        }
    }
    private static void DrawRectFilled(Rect2I rect, TileMapLayer tileMap, IEnumerator<Vector2I> colorEnum)
    {
        colorEnum.MoveNext();
        Vector2I tileSetCoords = colorEnum.Current;
        var enumetator = rect.GetEnumerator();
        while(enumetator.MoveNext())
            tileMap.SetCell(enumetator.Current, 0, tileSetCoords);
    }
    #region Pathfinding
    public static LevelRegion FindEnd(LevelSubdivision level)
    {
        LevelRegion end = level.First();

        foreach (var region in level)
        {
            if (region.Bounds.End.X > end.Bounds.End.X ||
               (region.Bounds.End.X == end.Bounds.End.X &&
                region.Bounds.Position.Y > end.Bounds.Position.Y))
                end = region;
        }

        return end;
    }
    public static LevelRegion FindStart(LevelSubdivision level)
    {
        LevelRegion start = level.First();

        foreach (var region in level)
        {
            if (region.Bounds.Position.X < start.Bounds.Position.X ||
               (region.Bounds.Position.X == start.Bounds.Position.X &&
                region.Bounds.Position.Y > start.Bounds.Position.Y))
                start = region;
        }

        return start;
    }
    public static Stack<LevelRegion> FindPath(LevelRegion start, Predicate<LevelRegion> endPredicate, LevelSubdivision subdivision)
    {
        HashSet<LevelRegion> visited = [];
        Stack<LevelRegion> stack = new();
        LevelRegion[] neighbors = new LevelRegion[subdivision.Length]; //Без аллокации B)

        stack.Push(start);
        visited.Add(start);

        while (true) //Если стак кончится мы всё равно получим ошибку далее 
        {
            LevelRegion current;
            current = stack.Peek();

            if (endPredicate(current))
                break;

            int neighborsCount = 0;
            float minY = current.Bounds.Position.Y, maxY = current.Bounds.End.Y;
            var neighborsEnum = current.Neighbors(horizontalPredicate: neighbor => Mathf.Min(neighbor.Bounds.End.Y,maxY) - Mathf.Max(neighbor.Bounds.Position.Y,minY) >= 2).GetEnumerator();
            while (neighborsEnum.MoveNext())
                if (!visited.Contains(neighborsEnum.Current))
                    neighbors[neighborsCount++] = neighborsEnum.Current; 

            if (neighborsCount > 0)
            {
                LevelRegion next = neighbors[Random.Shared.Next(neighborsCount)];
                stack.Push(next);
                visited.Add(next);
            }
            else
                stack.Pop();
        }

        return stack;
    }
    #endregion
}