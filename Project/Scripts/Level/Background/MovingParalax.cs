using Godot;

public partial class MovingParalax : Parallax2D
{
    private Level level;
    public override void _Ready() => level = GetNode<Level>("%Level");


    public override void _Process(double _)
    {
        var offset = level.Speed * ScrollScale * ((float)level.TimeSpent * 2);
        if (RepeatSize.X != 0)
        {
            float x = offset.X - RepeatSize.X;
            if (x > 0)
                offset.X = x;
        }
        if (RepeatSize.Y != 0)
        {
            float y = offset.Y - RepeatSize.Y;
            if (y > 0)
                offset.Y = y;
        }

        ScrollOffset = offset;
    }
}
