using System;
using _Config.So;
using MyBox;
using UnityEngine;
using XNode;

[CreateNodeMenu("Interaction"),NodeWidth(300),NodeTint(0.45f,0.35f,0.05f)]
public class InteractionNode : Node, IEpisodeGraphNode
{
    private enum Interactions
    {
        Orientation,
        Options
    }
    [Input(ShowBackingValue.Always,ConnectionType.Override),SerializeField] private OccasionNode _next;
    public string NodeName
    {
        get
        {
            var name = _next?.So?.name ?? "Undefined";
            return name + "_" + _interactionType;
        }
    }

    #region Connection
    public override void OnCreateConnection(NodePort from, NodePort to)
    {
        if (to.node == this && to.fieldName.StartsWith(nameof(_next))) 
            _next = from.node as OccasionNode;
        base.OnCreateConnection(from, to);
    }
    public override void OnRemoveConnection(NodePort port)
    {
        if (port.node == this && port.fieldName.StartsWith(nameof(_next))) 
            _next = null;
        base.OnRemoveConnection(port);
    }
    #endregion

    [SerializeField] private Interactions _interactionType;
    [ConditionalField(nameof(_interactionType),false,Interactions.Orientation),SerializeField] private RoleOrientation _orientation;
    [ConditionalField(nameof(_interactionType),false,Interactions.Options),SerializeField] private OptionsField _options;
    
    [Serializable] private class OptionsField
    {
        public Option Default;
        public Option[] Options;
    }
    [Serializable] private class Option
    {
        public string Label;
        public ResultSet[] Results;
    }
    [Serializable] private class RoleOrientation
    {
        public ResultSet[] Face;
        public ResultSet[] Back;
    }
    [Serializable] private class ResultSet
    {
        public PlotTagSoBase Tag;
        public double Value = 0;
    }
}