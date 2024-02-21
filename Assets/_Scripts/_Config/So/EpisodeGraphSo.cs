using System;
using System.Linq;
using _Data;
using MyBox;
using Sirenix.OdinInspector;
using UnityEngine;
using ReadOnly = MyBox.ReadOnlyAttribute;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "EpisodeSo", menuName = "配置/新/剧集")]
    public class EpisodeGraphSo : AutoNameGraphBase ,IDataElement
    {
        protected override char Separator => '_';
        [SerializeField]private int _id;
        [TextArea]public string Description;
        public int MaxRound = -1;

        public int Id => _id;

        public EpisodeData GetEpisode()
        {
            Debug.Log("未载入Occasion!");
            return new EpisodeData(_id, Description, Array.Empty<IOccasion>());
        }
    }

    public abstract class EpNodeBase : XNodeBase
    {
        public abstract string NodeName { get; }
        [OnInspectorGUI]
        private void CheckSoData() => name = string.IsNullOrWhiteSpace(NodeName) ? "Unknown" : NodeName;
    }

    public abstract class EpRoundNode : EpNodeBase
    {
        public OccasionSo Occasion;

        private string _nodeName;
        public override string NodeName
        {
            get
            {
                _nodeName = Occasion ? Occasion.name : "NotSet!";
                return _nodeName;
            }
        }

        [Input(ShowBackingValue.Never)] public EpNodeBase[] Prev;
        [Output(ShowBackingValue.Never,ConnectionType.Override)] public EpNodeBase Next;
        [Output] public EpNodeBase[] Options;
        [ReadOnly]public EpNodeBase[] PrevList;
        [ReadOnly]public EpNodeBase[] NextList;

        protected override void ConnectionNodeUpdate()
        {
            UpdatePortListConnection(nameof(Options), Options?.Length ?? 0,
                (p, i) => Options[i] = p.Connection.node as EpNodeBase,
                (_, i) => Options[i] = null);
            UpdatePortListConnection(nameof(Prev), Prev?.Length ?? 0,
                (p, i) => Prev[i] = p.Connection.node as EpNodeBase,
                (_, i) => Prev[i] = null);
            UpdatePortConnection(nameof(Next),
                p => Next = p.Connection.node as EpNodeBase,
                p => Next = null);
            var prevPort = GetPort(nameof(Prev));
            var nextPort = GetPort(nameof(Options));
            var nextList = nextPort.GetConnections().Select(c => c.node).Cast<EpNodeBase>().ToList();
            if (Next) nextList.Insert(0, Next);
            NextList = nextList.ToArray();
            PrevList = prevPort.GetConnections().Select(c => c.node).Cast<EpNodeBase>().ToArray();
        }
    }
}