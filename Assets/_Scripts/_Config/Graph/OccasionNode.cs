using System;
using _Config.So;
using _Data;
using UnityEngine;
using XNode;
using MyBox;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;
using ReadOnly = MyBox.ReadOnlyAttribute;

[CreateNodeMenu("Occasion"), NodeWidth(300), NodeTint(0.4f, 0.3f, 0.5f)]
public class OccasionNode : XNodeBase, IEpisodeGraphNode
{
    public string NodeName
    {
        get
        {
            var name = So?.name;
            return string.IsNullOrWhiteSpace(name) ? "Undefined" : name + "_Occasion";
        }
    }

    public OccasionSo So;

    [Output(ShowBackingValue.Always, ConnectionType.Override)]
    public EpisodeNode Node;

    #region Node Connection

    public override void OnCreateConnection(NodePort from, NodePort to)
    {
        OnPortConnected(from, nameof(Node), () => Node = to.node as EpisodeNode);
        OnPortConnected(from, nameof(SoloInteraction), () => SoloInteraction = to.node as InteractionNode);
        OnPortConnected(from, nameof(LeftInteraction), () => LeftInteraction = to.node as InteractionNode);
        OnPortConnected(from, nameof(RightInteraction), () => RightInteraction = to.node as InteractionNode);
        base.OnCreateConnection(from, to);
    }

    public override void OnRemoveConnection(NodePort port)
    {
        OnPortDisconnected(nameof(Node), () => Node = null);
        OnPortDisconnected(nameof(SoloInteraction), () => SoloInteraction = null);
        OnPortDisconnected(nameof(LeftInteraction), () => LeftInteraction = null);
        OnPortDisconnected(nameof(RightInteraction), () => RightInteraction = null);
        base.OnRemoveConnection(port);
        return;

    }

    #endregion

    private bool CheckIsSoExist() => So != null;

    [ConditionalField(true, nameof(CheckIsSoExist)), SerializeField, FormerlySerializedAs("Versus")]
    private RoleSet Set;

    public IOccasion GetOccasion()
    {
        if (!So) throw new NullReferenceException($"{name}.So is null!");
        var content = So.GetSceneContent();
        if (!Node) throw new NullReferenceException($"{name}.Node is null!");
        return new OccasionData(So.PlaceMode, So.Name, So.Description, content, Node);
    }

    [OnInspectorGUI]
    private void CheckSoData()
    {
        if (So == null) return;
        Set.Type = So.PlaceMode;
        switch (So.PlaceMode)
        {
            case Occasion.PlaceMode.Solo:
                Set.Solo.Type = So.GetInteractionType(Role.Index.Solo);
                _openLeftInteraction = false;
                _openRightInteraction = false;
                _openSoloInteraction = true;
                break;
            case Occasion.PlaceMode.Versus:
                Set.Left.Type = So.GetInteractionType(Role.Index.Left);
                Set.Right.Type = So.GetInteractionType(Role.Index.Right);
                _openLeftInteraction = true;
                _openRightInteraction = true;
                _openSoloInteraction = false;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private bool _openLeftInteraction;
    private bool _openRightInteraction;
    private bool _openSoloInteraction;

    [ShowIf(nameof(_openLeftInteraction))] [Output(ShowBackingValue.Always, ConnectionType.Override)]
    public InteractionNode SoloInteraction;

    [ShowIf(nameof(_openRightInteraction))] [Output(ShowBackingValue.Always, ConnectionType.Override)]
    public InteractionNode LeftInteraction;

    [ShowIf(nameof(_openSoloInteraction))] [Output(ShowBackingValue.Always, ConnectionType.Override)]
    public InteractionNode RightInteraction;

    [Serializable]
    private class RoleSet
    {
        [ReadOnly] public Occasion.PlaceMode Type;

        [ConditionalField(nameof(Type), false, Occasion.PlaceMode.Solo)]
        public SingleRoleSet Solo;

        [ConditionalField(nameof(Type), false, Occasion.PlaceMode.Versus)]
        public SingleRoleSet Left;

        [ConditionalField(nameof(Type), false, Occasion.PlaceMode.Versus)]
        public SingleRoleSet Right;
    }

    [Serializable]
    private class SingleRoleSet //交互设定
    {
        [ReadOnly] public Role.InteractionType Type; //交互类型

        [ConditionalField(nameof(Type), true, _Data.Role.InteractionType.Main), ReadOnly]
        public RoleSo Role; //交互角色
    }
}