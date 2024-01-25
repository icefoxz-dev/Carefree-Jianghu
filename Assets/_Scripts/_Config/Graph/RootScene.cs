using System.Linq;
using _Data;
using XNode;

[CreateNodeMenu("RootScene"), NodeTint(0.7f, 0.25f, 0f)] public class RootScene : EpisodeNode
{
    [Output(ShowBackingValue.Always, 
        ConnectionType.Override, 
        dynamicPortList = true)]
    public EpisodeNodeBase[] _next;

    public override Occasion.Phase Phase => _Data.Occasion.Phase.Root;

    public override IEpisodeNode[] GetNextNodes() => _next.Cast<IEpisodeNode>().ToArray();

    #region Node Connection
    public override void OnCreateConnection(NodePort from, NodePort to)
    {
        OnPortConnected(from,nameof(_next), () =>
        {
            var nextNode = (to.node as EpisodeNodeBase);
            nextNode?.SetPrevNode(GetNameIndex(to), this);
        });
        base.OnCreateConnection(from, to);
    }
    public override void OnRemoveConnection(NodePort port)
    {
        OnPortDisconnected(nameof(_next), () =>
        {
            var index = GetNameIndex(port);
            _next[index] = null;
        });
        base.OnRemoveConnection(port);
    }

    public override void SetPrevNode(int index, EpisodeNodeBase node) => throw new System.NotImplementedException("Root node should not set prev");
    public override void SetNextNode(int index, EpisodeNodeBase node) => _next[index] = node;
    #endregion
}