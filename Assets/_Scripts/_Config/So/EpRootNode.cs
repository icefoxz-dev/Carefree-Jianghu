using System.Linq;
using ReadOnly = MyBox.ReadOnlyAttribute;
namespace _Config.So
{
    [CreateNodeMenu("EpRoot"), NodeTint(0.31f, 0.34f, 0.26f)]
    public class EpRootNode : EpNodeBase
    {
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