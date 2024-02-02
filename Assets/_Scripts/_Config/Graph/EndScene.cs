using System;
using _Data;
using MyBox;
using UnityEngine;
using XNode;

[CreateNodeMenu("EndScene"),NodeTint(0.25f, 0.24f, 0.26f)] public class EndScene : ActiveEpisodeNode
{
    [Input(ShowBackingValue.Never),SerializeField,ReadOnly] private EpisodeNode _prev;

    public override Occasion.Phase Phase => Occasion.Phase.End;
    public override IEpisodeNode[] GetNextNodes() => Array.Empty<IEpisodeNode>();
}