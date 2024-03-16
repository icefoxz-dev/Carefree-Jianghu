using System;
using System.Linq;
using _Data;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace _Config.So
{
    public abstract class OccasionBase : AutoUnderscoreNamingObject, IOccasion
    {
        public abstract Occasion.Modes Mode { get; }
        public abstract string Description { get; }
        public abstract IChallengeArgs ChallengeArgs { get; }
        public abstract IRolePlacing[] GetPlacingInfos();
        public abstract string GetLine(RolePlacing.Index role, int index);
        public abstract ITagTerm[] GetExcludedTerms(IRoleData role);
        public abstract void CheckTags();
    }

    public abstract class PurposeOccasionBase : OccasionBase, IPurpose
    {
        [SerializeField] private string _title;
        [FormerlySerializedAs("Mode")] public Occasion.Modes _mode;

        [HideIf(nameof(_mode), Occasion.Modes.Solo)] [SerializeField]
        private InteractionSet Left;

        [HideIf(nameof(_mode), Occasion.Modes.Solo)] [SerializeField]
        private InteractionSet Right;

        [ShowIf(nameof(_mode), Occasion.Modes.Solo)] [SerializeField]
        private InteractionSet Solo;

        [TextArea, FormerlySerializedAs("Description")]
        public string _description;

        public override string Name => _title;
        public override Occasion.Modes Mode => _mode;
        public override string Description => _description;
        public abstract bool IsMandatory { get; }
        public IOccasion GetOccasion(IRoleData role) => this;

        public override string GetLine(RolePlacing.Index role, int index)
        {
            var line = role switch
            {
                RolePlacing.Index.Solo => Solo.Lines[index],
                RolePlacing.Index.Left => Left.Lines[index],
                RolePlacing.Index.Right => Right.Lines[index],
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
            };
            return line;
        }

        public override IRolePlacing[] GetPlacingInfos()
        {
            return _mode switch
            {
                Occasion.Modes.Solo => new IRolePlacing[]
                {
                    new RolePlacingInfo(RolePlacing.Index.Solo, Solo.PlaceMode, Solo.Role)
                },
                Occasion.Modes.Versus => new IRolePlacing[]
                {
                    new RolePlacingInfo(RolePlacing.Index.Left, Left.PlaceMode, Left.Role),
                    new RolePlacingInfo(RolePlacing.Index.Right, Right.PlaceMode, Right.Role),
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private class RolePlacingInfo : IRolePlacing
        {
            public RolePlacing.Index Place { get; }
            public RolePlacing.Modes Mode { get; }
            public ICharacter Character { get; }

            public RolePlacingInfo(RolePlacing.Index place, RolePlacing.Modes mode, ICharacter character)
            {
                Place = place;
                Mode = mode;
                Character = character;
            }
        }

        [Serializable]
        protected class InteractionSet //交互设定
        {
            [FormerlySerializedAs("Type")] public RolePlacing.Modes PlaceMode; //交互类型

            [ShowIf(nameof(PlaceMode), RolePlacing.Modes.Fixed)]
            public CharacterSo Role; //交互角色

            [TextArea] public string[] Lines;
        }


    }

    [Serializable]
    public class RewardTag : ITagValue
    {
        [SerializeField, FormerlySerializedAs("RoleTag")]
        public RoleTagSoBase _tag;

        [SerializeField] private double _value = 1;

        public double Value => _value;

        public IGameTag Tag => _tag;
        public string Name => _tag.Name;
        public TagType TagType => Tag.TagType;
    }

    [Serializable]
    public class BattleRewardJudge
    {
        [SerializeField] private RewardTag[] _win;
        [SerializeField] private RewardTag[] _lose;

        public ITagValue[] GetRewards(IOccasionResult result) =>
            result.Result switch
            {
                0 => _lose,
                _ => _win
            };

        public void CheckTags(Object o)
        {
            if (_win.Concat(_lose).Any(t => !t._tag))
                Debug.LogError("BattleSo CheckTags Failed", o);
        }
    }
}