using DustyStudios.MathAVM;
using Godot;
using System.Collections;
using System.Collections.Generic;

[GlobalClass]
public partial class LevelSubdivision : GodotObject, IEnumerable<LevelRegion>
{
    private List<LevelRegion> regions = new();

    public void Add(Rect2I region) => Add(new LevelRegion(region));
    public void Add(LevelRegion region)
    {
        region.subdivision = this;

        foreach (var item in regions)
            region.bounds.CutBy(item.bounds);
        UpdateNeighbors();

        regions.Add(region);
    }
    public void UpdateNeighbors()
    {
        foreach (var region in regions)
        {
            region.rightNeighbors.Clear();
            region.bottomNeighbors.Clear();
            region.leftNeighbors.Clear();
            region.topNeighbors.Clear();
            foreach (var item in regions)
                switch (region.bounds.GetTouchSide(item))
                {
                    case TouchSide.Left:
                        region.leftNeighbors.Add(item);
                        break;
                    case TouchSide.Right:
                        region.rightNeighbors.Add(item);
                        break;
                    case TouchSide.Top:
                        region.topNeighbors.Add(item);
                        break;
                    case TouchSide.Bottom:
                        region.bottomNeighbors.Add(item);
                        break;
                }
        }
    }
    public IEnumerator<LevelRegion> GetEnumerator() => regions.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => regions.GetEnumerator();

}
