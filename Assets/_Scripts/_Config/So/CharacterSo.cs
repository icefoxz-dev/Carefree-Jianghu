using System;
using System.Collections.Generic;
using System.Linq;
using _Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "CharacterSo", menuName = "配置/人物角色")]
    public class CharacterSo : AutoUnderscoreNamingObject, ICharacter,IRoleEquipSet
    {
        [SerializeField] private Com_Character _prefab;
        [SerializeField] [TextArea] private string _description;
        [SerializeField] private CombatSet _combat;
        [SerializeField] private SkillTagSet<ForceSkillTagSo> _force;
        [SerializeField] private SkillTagSet<DodgeSkillTagSo> _dodge;

        [SerializeField, FormerlySerializedAs("_traits")]
        private TagModel<TraitSo>[] 性格;
        [SerializeField] private TagModel<AbilitySo>[] 属性;
        [SerializeField] private TagModel<SkillTagSo>[] 技能;
        [SerializeField] private TagModel<StatusTagSo>[] 状态;
        [SerializeField] private TagModel<AbilitySo>[] 物品;
        private ITagSet Traits => new TagSet(性格);
        private ITagSet Abilities => new TagSet(属性);
        private ITagSet Inventory => new TagSet(物品);
        private IEnumerable<(ISkillTag skill,double value)> Skills => 技能.Select(s=> ((ISkillTag)s._so,s.Value));
        private IEnumerable<ITagStatus> Status => 状态.Select(s=>s._so.GetStatusTag());
        public ISkillSet<ICombatSkill> Combat => new SkillSet<ICombatSkill>(_combat.Tag, _combat.Level);
        public ISkillSet<ISkillTag> Force => new SkillSet<ISkillTag>(_force.Tag, _force.Level);
        public ISkillSet<ISkillTag> Dodge => new SkillSet<ISkillTag>(_dodge.Tag, _dodge.Level);
        public string Description => _description;
        public GameObject Prefab => _prefab.gameObject;

        public IRoleData GetRoleData(IEnumerable<IFormulaTag> capable) =>
            new RoleData(capable, Traits, Abilities, Status, Skills, Inventory, this);

        [Serializable] private class TagModel<T> : ValueTag where T : IRoleTag
        {
            public T _so;
            [Min(1)] public double _value;

            public override IGameTag Tag => _so;

            public override string Name => _so.Name;
            public override double Value => _value;
        }

        [Serializable]
        private class CombatSet
        {
            [SerializeField] private CombatSkillTagSo _tag;
            [SerializeField] private int _level;
            public ICombatSkill Tag => _tag;
            public int Level => _level;
        }

        [Serializable]
        private class SkillTagSet<T> where T : SkillTagSo
        {
            [SerializeField] private T _tag;
            [SerializeField] private int _level;
            public ISkillTag Tag => _tag;
            public int Level => _level;
        }

        private abstract class ValueTag : IValueTag
        {
            public abstract IGameTag Tag { get; }
            public abstract string Name { get; }
            public TagType TagType => Tag.TagType;
            public abstract double Value { get; }
        }
        private record TagSet(IEnumerable<IValueTag> Set) : ITagSet
        {
            public IEnumerable<IValueTag> Set { get; } = Set;
        }

        private record RoleData : IRoleData
        {
            public RoleData(IEnumerable<IFormulaTag> capable, ITagSet traits, ITagSet abilities,
                IEnumerable<ITagStatus> status, IEnumerable<(ISkillTag skill, double value)> skills, ITagSet inventory,
                CharacterSo character)
            {
                Attributes = new RoleAttributes(
                    capable: new FormulaTagManager(capable, this),
                    Trait: new TagManager(traits),
                    Ability: new TagManager(abilities),
                    Status: new StateTagManager(status),
                    Skill: new SkillTagManager(skills),
                    Inventory: new TagManager(inventory),
                    Story: new TagManager(Array.Empty<IGameTag>()));
                Character = character;
                EquipSet = character;
            }

            public IRoleAttributes Attributes { get; }
            public ICharacter Character { get; }
            public IRoleEquipSet EquipSet { get; }

            public void Proceed(IGameTag tag, double value)
            {
                throw new InvalidOperationException($"参考类型的角色不可以赋值！tag = {tag}");
            }

            public void Deconstruct(out IRoleAttributes Attributes, out ICharacter Character, out IRoleEquipSet EquipSet)
            {
                Attributes = this.Attributes;
                Character = this.Character;
                EquipSet = this.EquipSet;
            }
        }
        private record RoleAttributes : IRoleAttributes
        {
            public IFormulaTagManager Capable { get; }
            public ITagManager<IGameTag> Trait { get; }
            public ITagManager<IGameTag> Ability { get; }
            public ITagManager<IGameTag> Status { get; }
            public ISkillTagManager Skill { get; }
            public ITagManager<IGameTag> Inventory { get; }
            public ITagManager<IGameTag> Story { get; }

            public RoleAttributes(IFormulaTagManager capable,
                ITagManager<IGameTag> Trait,
                ITagManager<IGameTag> Ability,
                ITagManager<IGameTag> Status,
                ISkillTagManager Skill,
                ITagManager<IGameTag> Inventory,
                ITagManager<IGameTag> Story)
            {
                Capable = capable;
                this.Trait = Trait;
                this.Ability = Ability;
                this.Status = Status;
                this.Skill = Skill;
                this.Inventory = Inventory;
                this.Story = Story;
            }
        }
    }
}