using Godot;
[GlobalClass]
public partial class GalleryPainting : Resource
{
    [Export] public PackedScene Scene;
    [Export] public Vector2 Size;
    [Export] public int Count;
}