using System.Linq;
using _Data;
using UnityEngine;
using XNode;

[CreateNodeMenu("PhaseScene"), NodeTint(0.6f, 0.15f, 0.05f)]
public class PhaseScene : EpisodeNode
{
    [Input(ShowBackingValue.Always,ConnectionType.Override,dynamicPortList = true),SerializeField]private EpisodeNodeBase[] _prev;
    [Output(ShowBackingValue.Always,
         ConnectionType.Override,dynamicPortList = true)]
    public EpisodeNodeBase[] _next;
    public override Occasion.Phase Phase => _Data.Occasion.Phase.Transition;
    public override IEpisodeNode[] GetNextNodes() => _next.Cast<IEpisodeNode>().ToArray();

    #region Node Connection
    public override void OnCreateConnection(NodePort from, NodePort to)
    {
        if (to.node == this)
            OnPortConnected(to, nameof(_prev), () =>
            {
                var prevNode = from.node as EpisodeNodeBase;
                prevNode?.SetNextNode(GetNameIndex(from), this);
            });
        else if (from.node == this && from.fieldName.StartsWith(nameof(_next)))
            OnPortConnected(from, nameof(_next), () =>
            {
                var nextNode = to.node as EpisodeNodeBase;
                nextNode?.SetPrevNode(GetNameIndex(to), this);
            });
        base.OnCreateConnection(from, to);
    }
    public override void OnRemoveConnection(NodePort port)
    {
        OnPortDisconnected(nameof(_prev), () => _prev = null);
        OnPortDisconnected(nameof(_next), () => _next[GetNameIndex(port)] = null);
        base.OnRemoveConnection(port);
    }

    public override void SetPrevNode(int index, EpisodeNodeBase node) => _prev[index] = node;
    public override void SetNextNode(int index, EpisodeNodeBase node)=> _next[index] = node;
    #endregion
}