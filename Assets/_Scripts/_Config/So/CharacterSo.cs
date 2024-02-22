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
        private IEnumerable<RoleTag> Traits => 性格;
        private IEnumerable<RoleTag> Capable=> 属性;
        private IEnumerable<RoleTag> Skills=> 技能;
        private IEnumerable<RoleTag> Tags => Traits.Concat(Capable).Concat(Skills);

        public string Description => _description;
        public GameObject Prefab => _prefab.gameObject;

        public IRoleData GetRoleData()
        {
            return new RoleData(new PlayerProperty(
                new TagManager(Traits.Select(t => t.ToFuncTag())),
                new TagManager(Capable.Select(t => t.ToFuncTag())),
                new TagManager(Skills.Select(t => t.ToFuncTag())),
                new TagManager(Array.Empty<IFuncTag>()),
                new TagManager(Array.Empty<IFuncTag>())), 
                this);
        }

        [Serializable]
        private class RoleTag<T> : RoleTag where T : IGameTag
        {
            public T _so;
            [Min(1)] public double _value;

            public override IGameTag GameTag => _so;

            public override string Name => _so.Name;
            public override ITagManager GetTagManager(IRoleProperty property) => _so.GetTagManager(property);
            public override double Value => _value;
        }
        private abstract class RoleTag : IPlotTag
        {
            public abstract IGameTag GameTag { get; }
            public abstract string Name { get; }
            public abstract double Value { get; }
            public abstract ITagManager GetTagManager(IRoleProperty property);
            public IFuncTag ToFuncTag() => new FuncTag(GameTag, Value);
        }

        private record RoleData(IRoleProperty Prop, ICharacter Character) : IRoleData
        {
            public IRoleProperty Prop { get; } = Prop;
            public ICharacter Character { get; } = Character;
        }

        private record PlayerProperty(
            ITagManager Trait,
            ITagManager Capable,
            ITagManager Skill,
            ITagManager EpisodeTag,
            ITagManager ChapterTag) : IRoleProperty
        {
            public ITagManager Trait { get; } = Trait;
            public ITagManager Capable { get; } = Capable;
            public ITagManager Skill { get; } = Skill;
            public ITagManager EpisodeTag { get; } = EpisodeTag;
            public ITagManager ChapterTag { get; } = ChapterTag;
        }
    }
}