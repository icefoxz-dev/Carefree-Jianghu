using System;
using System.Linq;
using _Config.So;
using _Data;
using UnityEngine;
using XNode;
using MyBox;

[CreateNodeMenu("Plot"),NodeWidth(300), NodeTint(0.1f, 0.3f, 0.35f)]
public class Plot : EpisodeNodeBase,IEpisodeNode
{
    public override string NodeName => string.IsNullOrWhiteSpace(_next != null ? _next.NodeName : null) ? "Undefined" : _next.NodeName + "_Plot";
    public int Order => 优先;
    [Input(ShowBackingValue.Always,ConnectionType.Override), SerializeField] private EpisodeNodeBase _prev;
    [SerializeField] private PlotField[] _conditions;
    [Header("数值大优先")][SerializeField] private int 优先 = 1;
    [Output(ShowBackingValue.Always, ConnectionType.Override),SerializeField] private EpisodeNodeBase _next;

    #region IEpisode Delegate
    private IEpisodeNode Next => _next as IEpisodeNode;
    public Occasion.Phase Phase => Next.Phase;
    public IEpisodeNode[] GetNextNodes() => Next.GetNextNodes();
    public IOccasion Occasion => Next?.Occasion;
    #endregion

    public int GetEpNodeOrder(IRoleTerm role) =>
        _conditions.All(p => role.Tags.Any(t => p.IsInTerm(t, p.Compare))) ? Order : -1;
    public override void OnCreateConnection(NodePort from, NodePort to)
    {
        if (to.node == this && to.fieldName.StartsWith(nameof(_prev)))
        {
            var prevNode = from.node as EpisodeNodeBase;
            var nextPort = GetPort(nameof(_next))?.Connection;
            var nextNode = nextPort?.node as EpisodeNodeBase;
            prevNode?.SetNextNode(GetNameIndex(from), nextNode);
            nextNode?.SetPrevNode(GetNameIndex(nextPort), prevNode);
        }        
        if (from.node == this && from.fieldName.StartsWith(nameof(_next)))
        {
            var nextNode = to.node as EpisodeNodeBase;
            var prevPort = GetPort(nameof(_prev))?.Connection;
            var prevNode = prevPort?.node as EpisodeNodeBase;
            nextNode?.SetPrevNode(GetNameIndex(to), prevNode);
            prevNode?.SetNextNode(GetNameIndex(prevPort), nextNode);
        }
        base.OnCreateConnection(from, to);
    }

    public override void OnRemoveConnection(NodePort port)
    {
        if(port.node == this && port.fieldName.StartsWith(nameof(_next)))
        {
            var nextPort = GetPort(nameof(_next));
            _next = nextPort?.Connection?.node as EpisodeNodeBase;
            _next?.SetPrevNode(GetNameIndex(nextPort), null);
            _next = null;
        }
        if(port.node == this && port.fieldName.StartsWith(nameof(_prev)))
        {
            var prevPort = GetPort(nameof(_prev));
            _prev = prevPort?.Connection?.node as EpisodeNodeBase;
            _prev?.SetNextNode(GetNameIndex(prevPort), null);
            _prev = null;
        }
        base.OnRemoveConnection(port);
    }

    [Serializable]
    private class PlotField : IPlotTag
    {
        string IPlotTag.Name => PlotTag.Name;
        double IPlotTag.Value => Value;
        public PlotTagSoBase PlotTag;
        public PlotTagCompares Compare;
        [ConditionalField(nameof(Compare), true, PlotTagCompares.HasTag)] public double Value;
    }

    public override void SetPrevNode(int index, EpisodeNodeBase node) => _prev = node;
    public override void SetNextNode(int index, EpisodeNodeBase node) => _next = node;
}