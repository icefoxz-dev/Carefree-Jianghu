using System;
using System.Collections.Generic;
using System.Linq;
using _Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "CharacterSo", menuName = "配置/人物角色")]
    public class CharacterSo : AutoUnderscoreNamingObject,ICharacter
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] [TextArea] private string _description;
        [SerializeField,FormerlySerializedAs("_traits")] private RoleTag<TraitSo>[] 性格;
        [SerializeField]private RoleTag<CapableSo>[] 属性;
        [SerializeField]private RoleTag<SkillTagSo>[] 技能;
        private IEnumerable<RoleTag> Tags => 性格.Cast<RoleTag>().Concat(属性).Concat(技能);

        public string Description => _description;
        public GameObject Prefab => _prefab;

        [Serializable]
        private class RoleTag<T> : RoleTag where T : IGameTag
        {
            public T _so;
            [Min(1)] public double _value;

            public override IGameTag GameTag => _so;
            public override string Name => _so.Name;
            public override ITagManager GetTagManager(IPlayerProperty property) => _so.GetTagManager(property);
            public override double Value => _value;
        }
        private abstract class RoleTag : IPlotTag
        {
            public abstract IGameTag GameTag { get; }
            public abstract string Name { get; }
            public abstract double Value { get; }
            public abstract ITagManager GetTagManager(IPlayerProperty property);
        }
    }
}