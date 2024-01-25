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
        [FormerlySerializedAs("Type")]public Occasion.PlaceMode PlaceMode;
        [HideIf(nameof(PlaceMode), Occasion.PlaceMode.Solo)] [SerializeField] private InteractionSet Left;
        [HideIf(nameof(PlaceMode), Occasion.PlaceMode.Solo)] [SerializeField] private InteractionSet Right;
        [ShowIf(nameof(PlaceMode), Occasion.PlaceMode.Solo)] [SerializeField] private InteractionSet Solo;

        public Role.InteractionType GetInteractionType(Role.Index index) =>
            index switch
            {
                Role.Index.Solo => Solo.Type,
                Role.Index.Left => Left.Type,
                Role.Index.Right => Right.Type,
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
            };
        public SceneContent GetSceneContent() => _sceneContent;

        [TextArea] public string Description;
        [Serializable] private class InteractionSet //交互设定
        {
            public Role.InteractionType Type; //交互类型
            [ShowIf(nameof(Type), _Data.Role.InteractionType.Fixed)] public RoleSo Role; //交互角色
        }
    }
}