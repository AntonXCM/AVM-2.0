using Godot;

public partial class GeneratorOutput : Node, IGenerable
{
    [Export] TileMapLayer debugLayer, gameplayLayer;
    [Export] NodePath generatorPath;
    public override void _Ready() => Generate();
    public void Generate() => (GetNode(generatorPath) as Generator).Generate(debugLayer, gameplayLayer);
}
