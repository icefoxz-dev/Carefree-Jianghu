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
        [SerializeField,FormerlySerializedAs("_traits")] private TagModel<TraitSo>[] 性格;
        [SerializeField]private TagModel<AbilitySo>[] 属性;
        [SerializeField]private TagModel<BasicSkillTagSo>[] 技能;
        [SerializeField]private TagModel<StatusTagSo>[] 状态;
        [SerializeField]private TagModel<AbilitySo>[] 物品;
        private IEnumerable<RoleTag> Traits => 性格;
        private IEnumerable<RoleTag> Abilities=> 属性;
        private IEnumerable<RoleTag> Skills=> 技能;
        private IEnumerable<RoleTag> Inventory=> 物品;
        private IEnumerable<TagModel<StatusTagSo>> Status => 状态;


        public string Description => _description;
        public GameObject Prefab => _prefab.gameObject;

        public IRoleData GetRoleData(IEnumerable<IValueTag> capable)
        {
            return new RoleData(new PlayerAttributes(
                    Capable: new CapableTagManager(capable),
                    Trait: new TagManager(Traits.Select(t => t.ToValueTag())),
                    Ability: new TagManager(Abilities.Select(t => t.ToValueTag())),
                    Status: new StateTagManager(Status.Select(t => t._so.GetStatusTag())),
                    Skill: new TagManager(Skills.Select(t => t.ToValueTag())),
                    Inventory: new TagManager(Inventory.Select(t => t.ToValueTag())),
                    Story: new TagManager(Array.Empty<IValueTag>())
                ),
                this);
        }

        [Serializable]
        private class TagModel<T> : RoleTag where T : IRoleTag
        {
            public T _so;
            [Min(1)] public double _value;

            public override IRoleTag Tag => _so;

            public override string Name => _so.Name;
            public override ITagManager GetTagManager(IRoleAttributes attributes) => _so.GetTagManager(attributes);
            public override double Value => _value;
        }
        private abstract class RoleTag : IValueTag
        {
            public abstract IRoleTag Tag { get; }
            public abstract string Name { get; }
            public abstract double Value { get; }
            public abstract ITagManager GetTagManager(IRoleAttributes attributes);
            public IValueTag ToValueTag() => new ValueTag(Tag, Value);
        }

        private record RoleData(IRoleAttributes Attributes, ICharacter Character) : IRoleData
        {
            public IRoleAttributes Attributes { get; } = Attributes;
            public ICharacter Character { get; } = Character;
        }

        private record PlayerAttributes(
            ITagManager Capable,
            ITagManager Trait,
            ITagManager Ability,
            ITagManager Status,
            ITagManager Skill,
            ITagManager Inventory,
            ITagManager Story
            ) : IRoleAttributes
        {
            public ITagManager Capable { get; } = Capable;
            public ITagManager Trait { get; } = Trait;
            public ITagManager Ability { get; } = Ability;
            public ITagManager Status { get; } = Status;
            public ITagManager Skill { get; } = Skill;
            public ITagManager Inventory { get; } = Inventory;
            public ITagManager Story { get; } = Story;
        }
    }
}