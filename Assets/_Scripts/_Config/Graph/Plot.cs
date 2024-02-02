using System;
using System.Linq;
using _Config.So;
using _Data;
using UnityEngine;
using XNode;
using MyBox;
using UnityEngine.Serialization;

[CreateNodeMenu("Plot"), NodeWidth(300), NodeTint(0.41f, 0.13f, 0.24f)]
public class Plot : EpisodeGraphNode, IEpisodeNode
{
    public override string NodeName => string.IsNullOrWhiteSpace(_next != null ? _next.NodeName : null) ? "Undefined" : _next.NodeName + "_Plot";
    public int Order => 优先;
    [Input(ShowBackingValue.Always, ConnectionType.Override), SerializeField, ReadOnly] private EpisodeNode _prev;
    [SerializeField] private PlotField[] _conditions;
    [Header("数值大优先")][SerializeField] private int 优先 = 1;
    [Output(ShowBackingValue.Always, ConnectionType.Override), SerializeField, ReadOnly] private EpisodeNode _next;

    #region IEpisode Delegate
    private IEpisodeNode Next => _next as IEpisodeNode;
    public Occasion.Phase Phase => Next.Phase;
    public IEpisodeNode[] GetNextNodes() => Next.GetNextNodes();
    public IOccasion Occasion => Next?.Occasion;
    #endregion

    public int GetEpNodeOrder(IRoleTerm role) => _conditions.IsInTerm(role.Tags) ? Order : -1;

    #region Connection
    public override void UpdateNode()
    {
        UpdatePortConnection(nameof(_prev), p =>
        {
            var prevNode = p.Connection.node as EpisodeNode;
            _prev = prevNode;
            _next?.UpdateNode();
        }, p =>
        {
            var prevPort = GetPort(nameof(_prev));
            _prev = prevPort?.Connection?.node as EpisodeNode;
            _next?.UpdateNode();
        });        
        UpdatePortConnection(nameof(_next), p =>
        {
            var nextNode = p.Connection.node as EpisodeNode;
            _next = nextNode;
            _prev?.UpdateNode();
        }, p =>
        {
            var nextPort = GetPort(nameof(_next));
            _next = nextPort?.Connection?.node as EpisodeNode;
            _prev?.UpdateNode();
        });
    }
    #endregion

    [Serializable]
    private class PlotField : IPlotTerm
    {
        string IGameTag.Name => GameTag.Name;

        double IPlotTag.Value => Value;
        public GameTag GameTag;
        [FormerlySerializedAs("Compare"), SerializeField] public PlotTagClause _clause;
        PlotTagClause IPlotTerm.Clause => _clause;
        bool IPlotTerm.IsInTerm(IPlotTag other) => this.IsInTerm(other, _clause);

        public ITagManager GetTagManager(IPlayerProperty property) => GameTag.GetTagManager(property);
        [ConditionalField(nameof(_clause), true, PlotTagClause.HasTag)] public double Value;
    }
}