using System;
using _Data;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "OccasionSo", menuName = "配置/故事/场合")]
    public class OccasionSo : AutoUnderscoreNamingObject
    {
        [SerializeField]private SceneContent _sceneContent;
        [FormerlySerializedAs("Modes")]public Occasion.Modes Mode;
        [HideIf(nameof(Mode), Occasion.Modes.Solo)] [SerializeField] private InteractionSet Left;
        [HideIf(nameof(Mode), Occasion.Modes.Solo)] [SerializeField] private InteractionSet Right;
        [ShowIf(nameof(Mode), Occasion.Modes.Solo)] [SerializeField] private InteractionSet Solo;
        [TextArea] public string Description;

        public SceneContent SceneContent => _sceneContent;

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
        public (RolePlacing.Index index,RolePlacing.Modes mode,CharacterSo character)[] GetRolePlacingInfos()
        {
            return Mode switch
            {
                Occasion.Modes.Solo => new[]
                {
                    (RolePlacing.Index.Solo, Solo.PlaceMode, Solo.Role)
                },
                Occasion.Modes.Versus => new[]
                {
                    (RolePlacing.Index.Left, Left.PlaceMode, Left.Role),
                    (RolePlacing.Index.Right, Right.PlaceMode, Right.Role),
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public (RolePlacing.Modes, CharacterSo) GetRolePlacingInfo(RolePlacing.Index index)
        {
            return index switch
            {
                RolePlacing.Index.Solo => (Solo.PlaceMode, Solo.Role),
                RolePlacing.Index.Left => (Left.PlaceMode, Left.Role),
                RolePlacing.Index.Right => (Right.PlaceMode, Right.Role),
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
            };
        }

        [Serializable] private class InteractionSet //交互设定
        {
            [FormerlySerializedAs("Type")]public RolePlacing.Modes PlaceMode; //交互类型
            [ShowIf(nameof(PlaceMode), _Data.RolePlacing.Modes.Fixed)] public CharacterSo Role; //交互角色
            [TextArea] public string[] Lines;
        }
    }
}