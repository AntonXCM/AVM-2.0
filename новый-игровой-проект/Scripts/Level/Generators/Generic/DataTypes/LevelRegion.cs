using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DustyStudios.MathAVM;
using Godot;
using Godot.Collections;
[GlobalClass]
public partial class LevelRegion : GodotObject, IEnumerable<Vector2I>
{
    public LevelSubdivision Subdivision;
    public LevelRegion(Rect2I bounds) => Bounds = bounds;
    public Rect2I Bounds;
    #region Neighbor saving
    public Array<LevelRegion> LeftNeighbors = [], RightNeighbors = [], BottomNeighbors = [], TopNeighbors = [];
    public IEnumerable<LevelRegion> Neighbors()
    {
        foreach (var neighbor in LeftNeighbors)
            yield return neighbor;
        foreach (var neighbor in RightNeighbors)
            yield return neighbor;
        foreach (var neighbor in TopNeighbors)
            yield return neighbor;
        foreach (var neighbor in BottomNeighbors)
            yield return neighbor;
    }
    public IEnumerable<LevelRegion> Neighbors(Predicate<LevelRegion> horizontalPredicate)
    {
        foreach (var neighbor in LeftNeighbors)
            if(horizontalPredicate(neighbor))
                yield return neighbor;
        foreach (var neighbor in RightNeighbors)
            if(horizontalPredicate(neighbor))
                yield return neighbor;
        foreach (var neighbor in TopNeighbors)
            yield return neighbor;
        foreach (var neighbor in BottomNeighbors)
            yield return neighbor;
    }
    public Rect2I GetNeighborsWrapper() //Я тупой, зачем я это делал пол часа?
    {
        int minX = int.MaxValue,
            minY = int.MaxValue,
            maxX = int.MinValue,
            maxY = int.MinValue;
        AggregateSide(LeftNeighbors, isLeft: RightNeighbors.Count is not 0);
        AggregateSide(RightNeighbors, isRight: LeftNeighbors.Count is not 0);
        AggregateSide(TopNeighbors, isTop: BottomNeighbors.Count is not 0);
        AggregateSide(BottomNeighbors, isBottom: TopNeighbors.Count is not 0);

        void AggregateSide(Array<LevelRegion> arr, bool isBottom = false, bool isTop = false, bool isLeft = false, bool isRight = false)
        {
            foreach (LevelRegion neigh in arr)
            {
                Rect2I bounds = neigh.Bounds;
                Vector2I boundsEnd = bounds.End;
                if (!isRight && bounds.Position.X < minX)
                    minX = bounds.Position.X;
                if (!isLeft && boundsEnd.X > maxX)
                    maxX = boundsEnd.X;
                if (!isTop && bounds.Position.Y < minY)
                    minY = bounds.Position.Y;
                if (!isBottom && boundsEnd.Y > maxY)
                    maxY = boundsEnd.Y;
            }
        }
        if (maxX < minX || maxY < minY)
            return default;

        return new(position: new(minX, minY), size: new(maxX - minX, maxY - minY));
    }
    public Rect2I GetTransitionsWrapper()
    {
        int minX = int.MaxValue,
            minY = int.MaxValue,
            maxX = int.MinValue,
            maxY = int.MinValue;

        foreach (var transition in Transitions)
        {
            var bounds = transition.Bounds;
            if (bounds.Position.X < minX) minX = bounds.Position.X;
            if (bounds.Position.Y < minY) minY = bounds.Position.Y;
            var end = bounds.End;
            if (end.X > maxX) maxX = end.X;
            if (end.Y > maxY) maxY = end.Y;
        }

        if (minX is not int.MaxValue) 
            return new(position: new(minX, minY), size: new(maxX - minX, maxY - minY));
        return default;
    }
    public void ExpandLeft(int range)
    {
        Bounds.Size += Vector2I.Right * range;
        Bounds.Position += Vector2I.Left * range;

        foreach (var neighbor in LeftNeighbors)
            neighbor.Bounds.Size += Vector2I.Left * range;

        Subdivision.UpdateNeighbors();
    }
    public void ExpandRight(int range)
    {
        Bounds.Size += Vector2I.Right * range;

        foreach (var neighbor in RightNeighbors)
        {
            neighbor.Bounds.Size += Vector2I.Left * range;
            neighbor.Bounds.Position += Vector2I.Right * range;
        }
        Subdivision.UpdateNeighbors();
    }
    public void ExpandUp(int range)
    {
        Bounds.Size += Vector2I.Down * range;
        Bounds.Position += Vector2I.Up * range;

        foreach (var neighbor in TopNeighbors)
            neighbor.Bounds.Size += Vector2I.Up * range;

        Subdivision.UpdateNeighbors();
    }
    public void ExpandDown(int range)
    {
        Bounds.Size += Vector2I.Down * range;

        foreach (var neighbor in BottomNeighbors)
        {
            neighbor.Bounds.Size += Vector2I.Up * range;
            neighbor.Bounds.Position += Vector2I.Down * range;
        }

        Subdivision.UpdateNeighbors();
    }
    #endregion
    public Array<LevelRegion> Transitions = [];
    public bool IsStart, IsEnd;
    public void RequireTransition(LevelRegion region)
    {
        if (Neighbors().Contains(region))
            Transitions.Add(region);
    }
    public IEnumerator<Vector2I> GetEnumerator() => Bounds.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Bounds.GetEnumerator();
    public static implicit operator Rect2I(LevelRegion region) => region.Bounds;
}