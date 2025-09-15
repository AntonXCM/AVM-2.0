using DustyStudios.AVM2.Factories;
using DustyStudios.AVM2.UI;
using Godot;
using System;
using System.Collections.Generic;
public partial class AntivirusIndicatorUIFactory : AbstractSceneFactory
{
    [Export] public PackedScene energyIndicatorScene;
    public override void _Ready()
    {
        IEnergy ammoStat = (IEnergy)GetNode("../Energy");
        Dictionary<string, Func<Node>> overrides = new()
        {
            ["Energy"] = () => new StatIndicator(ammoStat, energyIndicatorScene)
        };

        Tests.Tests.CutByTest();

        Create(overrides);
    }
}