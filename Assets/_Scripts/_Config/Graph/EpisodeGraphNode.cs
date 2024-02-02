using _Config.So;
using System.Linq;
using _Data;
using MyBox;
using UnityEngine.Events;
using XNode;
using UnityEngine.Serialization;
using UnityEngine;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using ReadOnly = MyBox.ReadOnlyAttribute;

public abstract class XNodeBase : Node
{
    protected void UpdatePortConnection(string fieldName, Action<NodePort> hasConnectionAction, Action<NodePort> noConnectionAction)
    {
        var ownPort = GetPort(fieldName);
        if (!ownPort.fieldName.StartsWith(fieldName)) return;
        if (!ownPort.IsConnected)
        {
            noConnectionAction(ownPort);
            return;
        }
        hasConnectionAction(ownPort);
    }
    
    protected void UpdatePortListConnection(string fieldName,int arrayCount , Action<NodePort> hasConnectionAction, Action<NodePort> noConnectionAction)
    {
        for (var i = 0; i < arrayCount; i++) UpdatePortConnection($"{fieldName} {i}", hasConnectionAction, noConnectionAction);
    }

    #region Connection
    public override void OnCreateConnection(NodePort from, NodePort to)
    {
        UpdateNode();
        base.OnCreateConnection(from, to);
    }
    public override void OnRemoveConnection(NodePort port)
    {
        UpdateNode();
        base.OnRemoveConnection(port);
    }
    #endregion
    protected void OnPortConnected(NodePort ownPort, string fieldName, UnityAction action)
    {
        if (ownPort.node == this && ownPort.fieldName.StartsWith(fieldName)) action();
    }

    protected void OnPortDisconnected(string fieldName, UnityAction action)
    {
        var port = GetPort(fieldName);
        if (!port.IsConnected) action();
    }

    public abstract void UpdateNode();
}

public abstract class EpisodeGraphNode : XNodeBase, IEpisodeGraphNode
{
    public abstract string NodeName { get; }
}

public abstract class EpisodeNode : EpisodeGraphNode, IEpisodeNode, IOccasion
{
    private enum OccasionModes
    {
        Undefined,
        Solo,
        Versus,
    }
    /// <summary>
    /// 根据NodePort的名字获取索引, 规范是: "[fieldName][space][index]"
    /// </summary>
    /// <param name="port"></param>
    /// <returns></returns>
    protected int GetNameIndex(NodePort port) =>
        int.TryParse(port.fieldName.Split(' ').Last(), out var index) ? index : -1;
    public override string NodeName
    {
        get
        {
            var name = So?.name;
            return string.IsNullOrWhiteSpace(name) ? "Undefined" : name + "_Occasion";
        }
    }
    public OccasionSo So;
    [OnInspectorGUI]
    private void CheckSoData()
    {
        if (So)
            Mode = So.Mode switch
            {
                Occasion.Modes.Solo => OccasionModes.Solo,
                Occasion.Modes.Versus => OccasionModes.Versus,
                _ => throw new ArgumentOutOfRangeException()
            };
        else Mode = OccasionModes.Undefined;
    }
    [FormerlySerializedAs("Modes"), SerializeField, ReadOnly] private OccasionModes Mode;
    [Input(ShowBackingValue.Never, ConnectionType.Override), SerializeField] protected InteractionNode Left;
    [Input(ShowBackingValue.Never, ConnectionType.Override), SerializeField] protected InteractionNode Right;
    [Input(ShowBackingValue.Never, ConnectionType.Override), SerializeField] protected InteractionNode Solo;
    IOccasion IEpisodeNode.Occasion => GetOccasion();
    #region Interface
    public abstract Occasion.Phase Phase { get; }
    public abstract IEpisodeNode[] GetNextNodes();
    public virtual int GetEpNodeOrder(IRoleTerm role) => 0;
    public ICharacter GetCharacter(RolePlacing.Index place) => GetInterSet(place)?.Character?.GetCharacter();

    public Occasion.Modes Modes => So.Mode;
    public IEpisodeNode EpNode => this;
    public string Name => So.Name;
    public string Description => So.Description;

    public abstract IOccasionInteraction Interaction { get; }

    public ISceneContent SceneContent => So.SceneContent;

    public string GetLine(RolePlacing.Index role, int index) => So.GetLine(role, index);

    public IRoleInteraction GetRoleInteraction(RolePlacing.Index place)
    {
        var interaction = place switch
        {
            RolePlacing.Index.Solo => Solo,
            RolePlacing.Index.Left => Left,
            RolePlacing.Index.Right => Right,
            _ => throw new ArgumentOutOfRangeException(nameof(place), place, null)
        };
        return interaction;
    }
    public IOccasion GetOccasion()
    {
        if (!So) throw new NullReferenceException($"{name}.So is null!");
        return this;
    }
    #endregion
    protected InteractionNode? GetInterSet(RolePlacing.Index place)
    {
        var set = place switch
        {
            RolePlacing.Index.Solo => Solo,
            RolePlacing.Index.Left => Left,
            RolePlacing.Index.Right => Right,
            _ => throw new ArgumentOutOfRangeException(nameof(place), place, null)
        };
        return set;
    }

    protected EpisodeNode GetConnectionNodeFromPort(NodePort port)
    {
        var connectionPort = port.Connection;
        if (connectionPort.node is Plot plot)
        {
            connectionPort = port.fieldName.StartsWith("_prev") ? plot.GetPort("_prev").Connection : plot.GetPort("_next").Connection;
        }
        return connectionPort?.node as EpisodeNode;
    }

    public (RolePlacing.Modes, CharacterSo) GetOccasionByPort(NodePort port)
    {
        if (port.fieldName.StartsWith(nameof(Solo))) return So.GetRolePlacingInfo(RolePlacing.Index.Solo);
        if (port.fieldName.StartsWith(nameof(Left))) return So.GetRolePlacingInfo(RolePlacing.Index.Left);
        if (port.fieldName.StartsWith(nameof(Right))) return So.GetRolePlacingInfo(RolePlacing.Index.Right);
        throw new ArgumentOutOfRangeException(nameof(port), port, null);
    }
}

public abstract class ActiveEpisodeNode : EpisodeNode, IOccasionInteraction
{
    public override void UpdateNode()
    {
        UpdatePortConnection(nameof(Left), p => Left = p.Connection.node as InteractionNode, p => Left = null);
        UpdatePortConnection(nameof(Right), p => Right = p.Connection.node as InteractionNode, p => Right = null);
        UpdatePortConnection(nameof(Solo), p => Solo = p.Connection.node as InteractionNode, p => Solo = null);
    }
    public override IOccasionInteraction Interaction => this;
    public IRolePlacing[] GetPlacingInfos()
    {
        var infos = So.GetRolePlacingInfos();
        var list = new List<IRolePlacing>();
        foreach (var (index, mode, character) in infos)
        {
            list.Add(new RolePlacing(index, mode,
                Interaction.GetRoleInteraction(index)?.InteractionType ?? _Data.RolePlacing.Interactions.None,
                character?.GetCharacter()));
        }
        return list.ToArray();
    }
    private record RolePlacing(_Data.RolePlacing.Index Place, _Data.RolePlacing.Modes Mode, _Data.RolePlacing.Interactions Interaction, ICharacter Character) : IRolePlacing
    {
        public _Data.RolePlacing.Index Place { get; } = Place;
        public _Data.RolePlacing.Modes Mode { get; } = Mode;
        public _Data.RolePlacing.Interactions Interaction { get; } = Interaction;
        public ICharacter Character { get; } = Character;
    }
}
