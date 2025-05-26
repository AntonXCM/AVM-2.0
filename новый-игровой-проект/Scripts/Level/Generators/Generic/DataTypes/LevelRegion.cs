using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DustyStudios.MathAVM;
using Godot;

public partial class LevelRegion : RefCounted, IEnumerable<Vector2I>
{
    public LevelSubdivision subdivision;
    public LevelRegion(Rect2I bounds) => this.bounds = bounds;
    public Rect2I bounds;
    #region Neighbor saving
    public HashSet<LevelRegion> leftNeighbors = new(), rightNeighbors = new(), bottomNeighbors = new(), topNeighbors = new();
    public IEnumerable<LevelRegion> Neighbors()
    {
        foreach (var neighbor in leftNeighbors)
            yield return neighbor;
        foreach (var neighbor in topNeighbors)
            yield return neighbor;
        foreach (var neighbor in rightNeighbors)
            yield return neighbor;
        foreach (var neighbor in bottomNeighbors)
            yield return neighbor;
    }
    public void ExpandLeft(int range)
    {
        bounds.Size += Vector2I.Right * range;
        bounds.Position += Vector2I.Left * range;

        foreach (var neighbor in leftNeighbors)
            neighbor.bounds.Size += Vector2I.Left * range;

        subdivision.UpdateNeighbors();
    }
    public void ExpandRight(int range)
    {
        bounds.Size += Vector2I.Right * range;

        foreach (var neighbor in rightNeighbors)
        {
            neighbor.bounds.Size += Vector2I.Left * range;
            neighbor.bounds.Position += Vector2I.Right * range;
        }
        subdivision.UpdateNeighbors();
    }
    public void ExpandUp(int range)
    {
        bounds.Size += Vector2I.Down * range;
        bounds.Position += Vector2I.Up * range;

        foreach (var neighbor in topNeighbors)
            neighbor.bounds.Size += Vector2I.Up * range;

        subdivision.UpdateNeighbors();
    }
    public void ExpandDown(int range)
    {
        bounds.Size += Vector2I.Down * range;

        foreach (var neighbor in bottomNeighbors)
        {
            neighbor.bounds.Size += Vector2I.Up * range;
            neighbor.bounds.Position += Vector2I.Down * range;
        }

        subdivision.UpdateNeighbors();
    }
    #endregion
    public HashSet<LevelRegion> transitions = new();
    public void RequireTransition(LevelRegion region)
    {
        if (Neighbors().Contains(region))
            transitions.Add(region);
    }

    public IEnumerator<Vector2I> GetEnumerator() => bounds.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => bounds.GetEnumerator();
    public static implicit operator Rect2I(LevelRegion region) => region.bounds;
}