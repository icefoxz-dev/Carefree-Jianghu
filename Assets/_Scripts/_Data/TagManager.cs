using System;
using System.Collections.Generic;
using System.Linq;

namespace _Data
{
    public class SkillTagManager : ISkillTagManager
    {
        public IEnumerable<ISkillTag> Skills => _skills.Keys;
        private Dictionary<ISkillTag, double> _skills;
        public IEnumerable<IValueTag> Set => _skills.Select(s => new ValueTag(s.Key, s.Value));

        public IEnumerable<ISkillSet<ICombatSkill>> CombatSkills => _skills
            .Where(s => s.Key.SkillType == SkillType.Combat)
            .Select(s => new SkillSet<ICombatSkill>((ICombatSkill)s.Key, (int)s.Value));
        public IEnumerable<ISkillSet<IBasicSkill>> BasicSkills => _skills
            .Where(s => s.Key.SkillType == SkillType.Basic)
            .Select(s => new SkillSet<IBasicSkill>((IBasicSkill)s.Key, (int)s.Value));
        public IEnumerable<ISkillSet<ISkillTag>> ForceSkills => _skills
            .Where(s => s.Key.SkillType == SkillType.Force)
            .Select(s => new SkillSet<ISkillTag>(s.Key, (int)s.Value));
        public IEnumerable<ISkillSet<ISkillTag>> DodgeSkills => _skills
            .Where(s => s.Key.SkillType == SkillType.Dodge)
            .Select(s => new SkillSet<ISkillTag>(s.Key, (int)s.Value));

        public SkillTagManager(IEnumerable<(ISkillTag skill, double value)> skills)
        {
            _skills = skills.ToDictionary(s => s.skill, s => s.value);
        }

        public void UpdateTag(ISkillTag tag, double value)
        {
            var skill = _skills.Keys.FirstOrDefault(t => t == tag);
            if (skill == null)
            {
                _skills.Add(tag, 0);
                skill = tag;
            }
            _skills[skill] += value;
        }

        public double GetTagValue(IGameTag tag)
        {
            var skill = _skills.Keys.FirstOrDefault(s => s == tag);
            if (skill == null) return 0;
            return _skills[skill];
        }

        double ITagSet<ISkillTag>.GetTagValue(ISkillTag tag) => GetTagValue(tag);
    }

    public class FormulaTagManager : IFormulaTagManager
    {
        private readonly List<IFormulaTag> _tags;
        private readonly IRoleData _role;

        public IEnumerable<IValueTag> Set => _tags.Select(t => new ValueTag(t, t.GetValue(_role)));

        public IEnumerable<IFormulaTag> FormulaTags => _tags;

        public FormulaTagManager(IEnumerable<IFormulaTag> capable, IRoleData role)
        {
            _role = role;
            _tags = capable.ToList();
        }

        public double GetTagValue(IGameTag tag)
        {
            var f = _tags.FirstOrDefault(t => t == tag);
            return f?.GetValue(_role) ?? 0;
        }

        double ITagSet<IFormulaTag>.GetTagValue(IFormulaTag tag) => GetTagValue(tag);
    }
    public class StateTagManager : ITagManager<IGameTag>
    {
        private readonly List<TagStatus> _tags;

        public IEnumerable<IValueTag> Set => _tags.Select(t => new ValueTag(t, t.Value));

        public StateTagManager(IEnumerable<ITagStatus> tags)
        {
            _tags = tags.Select(t => t.ToStatusTag(t.Max, t.Min)).ToList();
        }

        public double GetTagValue(IGameTag tag)
        {
            var status = _tags.FirstOrDefault(t => t.Tag == tag);
            return status?.Value ?? 0;
        }

        public void UpdateTag(IGameTag tag, double value)
        {
            var t = _tags.FirstOrDefault(t => t.Tag == tag);
            if (t==null)
                throw new InvalidOperationException($"No such tag! {tag}, 状态必须预设！");
            t.Add(value);
        }

    }

    /// <summary>
    /// 标签管理类
    /// </summary>
    public class TagManager : ITagManager<IGameTag>
    {
        private readonly Dictionary<IGameTag, double> _tags;

        public IEnumerable<IValueTag> Set => _tags.Select(t => new ValueTag(t.Key, t.Value));

        public TagManager(ITagSet set)
        {
            _tags = set.Set.ToDictionary(t => (IGameTag)t, t => t.Value);
        }
        public TagManager(IEnumerable<IGameTag> tags)
        {
            _tags = tags.ToDictionary(t => t, _ => default(double));
        }

        public double GetTagValue(IGameTag tag) => _tags.GetValueOrDefault(tag, 0);

        public void UpdateTag(IGameTag tag, double value)
        {
            if (_tags.ContainsKey(tag))
                _tags.Add(tag, 0);
            _tags[tag] += value;
        }
    }

    //标签状态类，Value会变化
    public record ValueTag : IValueTag
    {
        private readonly IGameTag _tag;
        private double _value;
        public IGameTag Tag => _tag;
        public string Name => Tag.Name;
        public TagType TagType => _tag.TagType;
        public double Value => _value;

        public ValueTag(IGameTag tag, double value = 0)
        {
            _tag = tag;
            _value = value;
        }
        public void AddValue(double v) => _value += v;
        public override string ToString() => $"{Name}: {Value}";
    }

    public record SkillSet<T>(T Tag, int Level = 1) : ISkillSet<T>
        where T : ISkillTag
    {
        public T Tag { get; } = Tag;
        public int Level { get; private set; } = Level;

        public void AddLevel(int level) => Level += level;
        public void SetLevel(int level) => Level = level;
        public double GetPower(IRoleData role) => Tag.GetPower(Level, role);
        public override string ToString() => $"{Tag.Name}:{Level}";
    }
}