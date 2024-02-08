using System.Linq;
using MyBox;

namespace _Config.So
{
    [CreateNodeMenu("EpNext"), NodeTint(0.45f, 0.35f, 0.25f)]
    public class EpNextNode : EpNodeBase
    {
        public override string NodeName => "Next";
        [Input(ShowBackingValue.Never)] public EpRoundNode[] Prev;
        [ReadOnly]public EpRoundNode[] PrevList;

        protected override void ConnectionNodeUpdate()
        {
            UpdatePortListConnection(nameof(Prev), Prev?.Length ?? 0,
                (p, i) => Prev[i] = p.Connection.node as EpRoundNode,
                (_, i) => Prev[i] = null);
            var port = GetPort(nameof(Prev));
            PrevList = port.GetConnections().Select(c => c.node).Cast<EpRoundNode>().ToArray();
        }
    }
}