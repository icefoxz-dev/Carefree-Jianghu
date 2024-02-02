using System.Linq;
using _Data;
using MyBox;
using UnityEngine;
using XNode;

[CreateNodeMenu("PhaseScene"), NodeTint(0.30f, 0.36f, 0.4f)]
public class PhaseScene : ActiveEpisodeNode
{
    [Input(ShowBackingValue.Never), SerializeField, ReadOnly] private EpisodeNode _prev;
    [Output(ShowBackingValue.Always, ConnectionType.Override, dynamicPortList = true)]
    [ReadOnly] public EpisodeNode[] _next;
    public override Occasion.Phase Phase => _Data.Occasion.Phase.Transition;
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

    #region Node Connection

    public override void UpdateNode()
    {
        base.UpdateNode();
        UpdatePortConnection(nameof(_next), p =>
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
    #endregion
}