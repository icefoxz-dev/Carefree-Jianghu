using System;
using _Data;
using UnityEngine;
using XNode;

[CreateNodeMenu("EndScene"),NodeTint(0.5f, 0f, 0f)] public class EndScene : EpisodeNode
{
    [Input(ShowBackingValue.Always,ConnectionType.Override,dynamicPortList = true),SerializeField] private EpisodeNodeBase[] _prev;

    public override Occasion.Phase Phase => _Data.Occasion.Phase.End;
    public override IEpisodeNode[] GetNextNodes() => Array.Empty<IEpisodeNode>();

    #region Node Connection
    public override void OnCreateConnection(NodePort from, NodePort to)
    {
        OnPortConnected(to, nameof(_prev), () =>
        {
            var prevNode = from.node as EpisodeNodeBase;
            prevNode?.SetNextNode(GetNameIndex(from), this);
        });
        base.OnCreateConnection(from, to);
    }

    public override void OnRemoveConnection(NodePort port)
    {
        OnPortDisconnected(nameof(_prev), () => _prev = null);
        base.OnRemoveConnection(port);
    }

    public override void SetPrevNode(int index, EpisodeNodeBase node) => _prev[index] = node;
    public override void SetNextNode(int index, EpisodeNodeBase node) => throw new System.NotImplementedException($"End node should not set Next! index = {index}");
    #endregion
}