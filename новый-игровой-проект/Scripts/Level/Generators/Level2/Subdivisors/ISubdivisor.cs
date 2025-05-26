using Godot;

public interface ISubdivisor
{
    public LevelSubdivision Subdivide(Rect2I rect);
}