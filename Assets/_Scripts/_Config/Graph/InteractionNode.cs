using System;
using System.Linq;
using _Config.So;
using _Data;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;
using XNode;

[CreateNodeMenu("Interaction"),NodeWidth(300),NodeTint(0.45f,0.35f,0.25f)]
public class InteractionNode : XNodeBase, IEpisodeGraphNode, IRoleInteraction
{
    public string NodeName
    {
        get
        {
            var name = _scene?.So?.name ?? "Undefined";
            return name + "_" + _interactionType;
        }
    }

    public override void UpdateNode()
    {
        UpdatePortConnection(nameof(_scene), p =>
        {
            var o = p.Connection;
            var scene = o.node as ActiveEpisodeNode;
            _scene = scene;
            if (!scene) return;
            var (placeMode, character) = scene.GetOccasionByPort(o);
            PlaceMode = placeMode;
            Character = character;
        }, p =>
        {
            _scene = null;
            Character = null;
        });
    }

    [SerializeField] private RolePlacing.Interactions _interactionType;
    [ConditionalField(nameof(_interactionType), false, RolePlacing.Interactions.Orientation), SerializeField] private RoleOrientation _orientation;
    [ConditionalField(nameof(_interactionType), false, RolePlacing.Interactions.Options), SerializeField] private OptionsField _options;
    [ConditionalField(true,nameof(CheckScene)),FormerlySerializedAs("Type"),ReadOnly] public RolePlacing.Modes PlaceMode; //交互类型
    private bool CheckScene() => _scene;
    [ConditionalField(nameof(PlaceMode), false, RolePlacing.Modes.Fixed),ReadOnly] public CharacterSo Character; //交互角色
    [Output(ShowBackingValue.Never, ConnectionType.Override), SerializeField] private EpisodeNode _scene;
    public RolePlacing.Interactions InteractionType => _interactionType;

    public IInteractionOption[] GetOptions()
    {
        return _interactionType switch
        {
            RolePlacing.Interactions.None => Array.Empty<IInteractionOption>(),
            RolePlacing.Interactions.Orientation => InteractionOption.FrontBack,
            RolePlacing.Interactions.Options => _options.Options.Select((o, i) => new InteractionOption(o.Label, i))
                .Cast<IInteractionOption>().ToArray(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public IFuncTag[] GetTags(IInteractionOption op) =>
        _interactionType switch
        {
            RolePlacing.Interactions.Orientation => op.SelectionIndex == 0 ?
                _orientation.Front :
                _orientation.Back,
            RolePlacing.Interactions.Options => _options.Options[op.SelectionIndex].Results,
            _ => throw new ArgumentOutOfRangeException()
        };

    public override object GetValue(NodePort port) => port.fieldName.StartsWith(nameof(_scene)) ? _scene : base.GetValue(port);

    private readonly struct InteractionOption : IInteractionOption
    {
        public static IInteractionOption[] FrontBack => new IInteractionOption[] {new InteractionOption(RolePlacing.Facing.Front),new InteractionOption(RolePlacing.Facing.Back)};
        public string Label { get; }
        public int SelectionIndex { get; }

        public InteractionOption(string label, int selectionIndex)
        {
            Label = label;
            SelectionIndex = selectionIndex;
        }

        private InteractionOption(RolePlacing.Facing facing)
        {
            Label = facing.ToString();
            SelectionIndex = (int)facing;
        }
    }

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
        public ResultSet[] Front;
        public ResultSet[] Back;
    }
    [Serializable] private class ResultSet : IFuncTag
    {
        public GameTag Tag;
        public double Value = 0;
        double IPlotTag.Value => Value;
        public string Name => Tag.Name;
        public ITagManager GetTagManager(IPlayerProperty property) => Tag.GetTagManager(property);

        public void SetPlayer(IPlayerData player) => Tag.GetTagManager(player.Prop).AddTag(this);
    }
}