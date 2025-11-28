using Godot;
using Godot.Collections;
public partial class GalleryGenerator : Node2D
{
    [Export] Vector2 wallRect;
    [Export] Vector2 gapRange;
    [Export] private Array<GalleryPainting> paintings;
    private Array<GalleryPainting> notPlaced;
    private readonly Array<Rect2> placed = new();
    public override void _Ready()
    {
        notPlaced = paintings.Duplicate(true);
        for (int i = 0; i < notPlaced.Count; i++)
            notPlaced[i] = (GalleryPainting)notPlaced[i].Duplicate();

        TryPlaceAt(notPlaced.PickRandom(), AnyRandomPosition(Vector2.One));
    }


    public override void _PhysicsProcess(double delta)
    {
        if(notPlaced.Count > 0)
            TryPlacePicture(notPlaced.PickRandom());
    }


    private void TryPlacePicture(GalleryPainting painting)
    {
        for (int tries = 0; tries < 50; tries++)
            if (TryPlaceAt(painting, AnyRandomPosition(painting.Size)))
                return;
        SetPhysicsProcess(false);
    }

    private Vector2 AnyRandomPosition(Vector2 paintingSize)
    {
        if (placed.Count is 0) return WallRandomPosition();
        var baseRect = placed.PickRandom();
        float randomLerp = Mathf.Lerp(gapRange.X, gapRange.Y, GD.Randf());
        return (GD.Randi() % 5) switch
        {
            0 => baseRect.Position + new Vector2(0, -paintingSize.Y - randomLerp),
            1 => baseRect.Position + new Vector2(0, randomLerp + baseRect.Size.Y),
            2 => baseRect.Position + new Vector2(-paintingSize.X - randomLerp, 0),
            3 => baseRect.Position + new Vector2(randomLerp + baseRect.Size.X, 0),
            _ => WallRandomPosition()
        };
        Vector2 WallRandomPosition() => new Vector2(GD.Randf(), GD.Randf()) * wallRect;
    }
    private bool TryPlaceAt(GalleryPainting painting, Vector2 position)
    {
        if (position.X < 0 || position.X + painting.Size.X > wallRect.X ||
            position.Y < 0 || position.Y + painting.Size.Y > wallRect.Y)
            return false;

        var rect = new Rect2(position, painting.Size);
        foreach (var r in placed)
            if (rect.Intersects(r))
                return false;
        placed.Add(rect);
        var inst = painting.Scene.Instantiate<Node2D>();
        if ((--painting.Count) <= 0) notPlaced.Remove(painting);
        inst.Position = (position * 16).Floor();
        AddChild(inst);
        return true;
    }
    // static IEnumerator<string> GetNiceCancellationToken = NiceCancellationTokenGenerator();
    // static IEnumerator<string> NiceCancellationTokenGenerator()
    // {
    // Start:
    //     yield return "Дружеская Выставка игр";
    //     yield return "Kitket427";
    //     yield return "Сфера жизни";
    //     yield return "Car";
    //     yield return "Slime King";
    //     yield return "Little AD agent";
    //     yield return "PWVerse";
    //     yield return "Lokich";
    //     yield return "Moveset";
    //     yield return "Ipatiy Potiy";
    //     yield return "Lough";
    //     yield return "City";
    //     yield return "К О Л Б А";
    //     yield return "Jon";
    //     yield return "No aitman allowed";
    //     yield return "LLM";
    //     yield return "Вадик и Ваня";
    //     yield return "Президент";
    //     yield return "SXR";
    //     yield return "Tower";
    //     goto Start;
    // }
}