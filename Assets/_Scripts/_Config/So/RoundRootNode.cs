using System.Linq;
using MyBox;
using UnityEngine;

namespace _Config.So
{
    [CreateNodeMenu("RoundRoot"), NodeTint(0.21f, 0.3f, 0.26f)]
    public class RoundRootNode : EpNodeBase
    {
        [SerializeField] private RoundTriggers roundTrigger;
        [ConditionalField(nameof(roundTrigger), false, RoundTriggers.RoundCount), SerializeField] private RoundCountSet _roundSet;
        [ConditionalField(nameof(roundTrigger), false, RoundTriggers.GameTag), SerializeField] private TagSet _tagSet;
        [Output] public EpRoundNode[] Next;
        [ReadOnly] public EpRoundNode[] NextList;
        public override string NodeName => "Root";

        protected override void ConnectionNodeUpdate()
        {
            UpdatePortListConnection(nameof(Next), Next?.Length ?? 0,
                (p, i) => Next[i] = p.Connection.node as EpRoundNode,
                (_, i) => Next[i] = null);
            var port = GetPort(nameof(Next));
            NextList = port.GetConnections().Select(c => c.node).Cast<EpRoundNode>().ToArray();
        }
    }
}