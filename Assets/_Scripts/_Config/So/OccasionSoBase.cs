using System;
using _Data;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Config.So
{
    public class OccasionSoBase : AutoUnderscoreNamingObject, IOccasion
    {
        [FormerlySerializedAs("Modes")]public Occasion.Modes Mode;
        [HideIf(nameof(Mode), Occasion.Modes.Solo)] [SerializeField] private InteractionSet Left;
        [HideIf(nameof(Mode), Occasion.Modes.Solo)] [SerializeField] private InteractionSet Right;
        [ShowIf(nameof(Mode), Occasion.Modes.Solo)] [SerializeField] private InteractionSet Solo;
        [TextArea] public string Description;
        Occasion.Modes IOccasion.Mode => Mode;
        string IOccasion.Description => Description;
        public virtual IFuncTag[] Results => Array.Empty<IFuncTag>();
        public string GetLine(RolePlacing.Index role,int index)
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

        public IRolePlacing[] GetPlacingInfos()
        {
            return Mode switch
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
            [FormerlySerializedAs("Type")]public RolePlacing.Modes PlaceMode; //交互类型
            [ShowIf(nameof(PlaceMode), RolePlacing.Modes.Fixed)] public CharacterSo Role; //交互角色
            [TextArea] public string[] Lines;
        }
    }
}