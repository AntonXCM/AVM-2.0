using DustyStudios.MathAVM;
using Godot;
using System.Collections;
using System.Collections.Generic;

[GlobalClass]
public partial class LevelSubdivision : GodotObject, IEnumerable<LevelRegion>
{
    private List<LevelRegion> regions = [];
    public int Length = 0;

    public void Add(Rect2I region) => Add(new LevelRegion(region));
    public void Add(LevelRegion region)
    {
        Length++;

        region.Subdivision = this;

        foreach (var item in regions)
            region.Bounds.CutBy(item.Bounds);
        UpdateNeighbors();

        regions.Add(region);
    }
    public void UpdateNeighbors()
    {
        foreach (var region in regions)
        {
            region.RightNeighbors.Clear();
            region.BottomNeighbors.Clear();
            region.LeftNeighbors.Clear();
            region.TopNeighbors.Clear();
            foreach (var item in regions)
                switch (region.Bounds.GetTouchSide(item))
                {
                    case TouchSide.Left:
                        region.LeftNeighbors.Add(item);
                        break;
                    case TouchSide.Right:
                        region.RightNeighbors.Add(item);
                        break;
                    case TouchSide.Top:
                        region.TopNeighbors.Add(item);
                        break;
                    case TouchSide.Bottom:
                        region.BottomNeighbors.Add(item);
                        break;
                }
        }
    }
    public IEnumerator<LevelRegion> GetEnumerator() => regions.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => regions.GetEnumerator();

    public Godot.Collections.Array<LevelRegion> get_regions() => [.. regions];
}
