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
        [SerializeField] private Com_Character _prefab;
        [SerializeField] [TextArea] private string _description;
        [SerializeField,FormerlySerializedAs("_traits")] private RoleTag<TraitSo>[] 性格;
        [SerializeField]private RoleTag<CapableSo>[] 属性;
        [SerializeField]private RoleTag<BasicSkillTagSo>[] 技能;
        [SerializeField]private RoleTag<StatusTagSo>[] 状态;
        [SerializeField]private RoleTag<CapableSo>[] 物品;
        private IEnumerable<RoleTag> Traits => 性格;
        private IEnumerable<RoleTag> Capable=> 属性;
        private IEnumerable<RoleTag> Skills=> 技能;
        private IEnumerable<RoleTag> Inventory=> 物品;
        private IEnumerable<RoleTag<StatusTagSo>> Status => 状态;
        private IEnumerable<RoleTag> Tags => Traits.Concat(Capable).Concat(Skills);

        public string Description => _description;
        public GameObject Prefab => _prefab.gameObject;

        public IRoleData GetRoleData()
        {
            return new RoleData(new PlayerAttributes(
                    Trait: new TagManager(Traits.Select(t => t.ToFuncTag())),
                    Capable: new TagManager(Capable.Select(t => t.ToFuncTag())),
                    Status: new StateTagManager(Status.Select(t => t._so.GetStatusTag())),
                    Skill: new TagManager(Skills.Select(t => t.ToFuncTag())),
                    Inventory: new TagManager(Inventory.Select(t => t.ToFuncTag())),
                    Story: new TagManager(Array.Empty<IValueTag>())
                ),
                this);
        }

        [Serializable]
        private class RoleTag<T> : RoleTag where T : IGameTag
        {
            public T _so;
            [Min(1)] public double _value;

            public override IGameTag GameTag => _so;

            public override string Name => _so.Name;
            public override ITagManager GetTagManager(IRoleAttributes attributes) => _so.GetTagManager(attributes);
            public override double Value => _value;
        }
        private abstract class RoleTag : IValueTag
        {
            public abstract IGameTag GameTag { get; }
            public abstract string Name { get; }
            public abstract double Value { get; }
            public abstract ITagManager GetTagManager(IRoleAttributes attributes);
            public IValueTag ToFuncTag() => new ValueTag(GameTag, Value);
        }

        private record RoleData(IRoleAttributes Attributes, ICharacter Character) : IRoleData
        {
            public IRoleAttributes Attributes { get; } = Attributes;
            public ICharacter Character { get; } = Character;
        }

        private record PlayerAttributes(
            ITagManager Trait,
            ITagManager Capable,
            ITagManager Status,
            ITagManager Skill,
            ITagManager Inventory,
            ITagManager Story) : IRoleAttributes
        {
            public ITagManager Trait { get; } = Trait;
            public ITagManager Capable { get; } = Capable;
            public ITagManager Status { get; } = Status;
            public ITagManager Skill { get; } = Skill;
            public ITagManager Inventory { get; } = Inventory;
            public ITagManager Story { get; } = Story;
        }
    }
}