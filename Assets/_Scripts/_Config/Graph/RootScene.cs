using System.Linq;
using _Data;
using MyBox;
using UnityEditor.Experimental.GraphView;
using XNode;

[CreateNodeMenu("RootScene"), NodeTint(0.31f, 0.34f, 0.26f)]
public class RootScene : ActiveEpisodeNode
{
    [Output(ShowBackingValue.Always,
        ConnectionType.Override,
        dynamicPortList = true)]
    [ReadOnly]
    public EpisodeNode[] _next;

    public override Occasion.Phase Phase => Occasion.Phase.Root;

    public override IEpisodeNode[] GetNextNodes() => _next.Cast<IEpisodeNode>().ToArray();

    public override object GetValue(NodePort port)
    {
        if (port.fieldName.StartsWith(nameof(_next)))
        {
            var index = GetNameIndex(port);
            return index >= 0 ? _next[index] : null;
        }
        return base.GetValue(port);
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
        UpdatePortListConnection(nameof(_next), _next.Length, p =>
        {
            var index = GetNameIndex(p);
            if (index < 0) return;
            _next[index] = GetConnectionNodeFromPort(p);
        }, p =>
        {
            var index = GetNameIndex(p);
            if (index < 0) return;
            _next[index] = null;
        });
    }

}