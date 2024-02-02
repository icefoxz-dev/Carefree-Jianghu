using System;
using _Data;
using MyBox;
using UnityEngine;
using XNode;

[CreateNodeMenu("GameOverScene"),NodeTint(0.5f, 0f, 0f)] public class GameOver : EpisodeNode
{
    [Input(ShowBackingValue.Never),SerializeField,ReadOnly] private EpisodeNode _prev;
    public override Occasion.Phase Phase => Occasion.Phase.End;
    public override IEpisodeNode[] GetNextNodes() => Array.Empty<IEpisodeNode>();
    public override IOccasionInteraction Interaction => null;

    public override void UpdateNode() { }
}