using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace _Data
{
    public class StateTagManager : ITagManager
    {
        private readonly List<StatusTag> _tags;
        public IEnumerable<IValueTag> Tags => _tags;

        public StateTagManager(IEnumerable<IStatusTag> tags)
        {
            _tags = tags.Select(s => s.ToStatusTag(s.Max, s.Min)).ToList();
        }


        public void AddTag(IStatusTag tag)
        {
            var t = GetFirstOrDefault(tag.GameTag);
            if (t != null) throw new DuplicateNameException($"tag.{tag.Name} already exist!");
            _tags.Add(tag.ToStatusTag());
        }

        public void RemoveTag(IValueTag tag)
        {
            var t = GetFirstOrDefault(tag.GameTag);
            _tags.Remove(t);
        }

        private StatusTag GetFirstOrDefault(IGameTag tag) => _tags.FirstOrDefault(t => t.Name == tag.Name);

        public void AddTagValue(IValueTag tag)
        {
            var t = GetFirstOrDefault(tag.GameTag);
            if (t == default)
                throw new NullReferenceException($"tag.{tag.Name} not exist!");
            t.Add(tag.Value);
        }

        public double GetTagValue(IGameTag tag, bool throwErrorIfNoTag = false)
        {
            var t = _tags.FirstOrDefault(t => t.GameTag == tag);
            if (t == null && throwErrorIfNoTag) throw new NoNullAllowedException($"tag.{tag.Name} not exist!");
            return t?.Value ?? 0;
        }
    }
    /// <summary>
    /// 标签管理类
    /// </summary>
    public class TagManager : ITagManager
    {
        private List<ValueTag> _tags;

        public TagManager(ITagManager tagManager) : this(tagManager.Tags)
        {
        }

        public TagManager(IEnumerable<IValueTag> tags)
        {
            _tags = tags.Select(t => new ValueTag(t, copyValue: true)).ToList();
        }

        public IEnumerable<IValueTag> Tags => _tags;

        public void AddTag(IGameTag tag)
        {
            var t = GetFirstOrDefault(tag);
            if (t != null) throw new DuplicateNameException($"tag.{tag.Name} already exist!");
            _tags.Add(new ValueTag(tag));
        }

        public void RemoveTag(IGameTag tag)
        {
            var t = GetFirstOrDefault(tag);
            _tags.Remove(t);
        }

        private ValueTag GetFirstOrDefault(IGameTag tag) => _tags.FirstOrDefault(t => t.Name == tag.Name);

        public void AddTagValue(IValueTag tag)
        {
            var t = GetFirstOrDefault(tag.GameTag);
            if (t == default)
            {
                AddTag(tag.GameTag);
                t = GetFirstOrDefault(tag.GameTag);
            }
            t.AddValue(tag.Value);
        }

        public double GetTagValue(IGameTag tag, bool throwErrorIfNoTag = false)
        {
            var t = _tags.FirstOrDefault(t => t.GameTag == tag);
            if (t == null && throwErrorIfNoTag) throw new NoNullAllowedException($"tag.{tag.Name} not exist!");
            return t?.Value ?? 0;
        }
    }

    //标签状态类，Value会变化
    public record ValueTag : IValueTag
    {
        private readonly IGameTag gameTag;

        public IGameTag GameTag => gameTag;

        public string Name => GameTag.Name;
        public double Value { get; private set; }

        public ValueTag(IValueTag tag, bool copyValue = false)
        {
            gameTag = tag.GameTag;
            if (copyValue) Value = tag.Value;
        }

        public ValueTag(IGameTag tag, double value = 0)
        {
            gameTag = tag;
            Value = value;
        }

        public ITagManager GetTagManager(IRoleAttributes attributes) => GameTag.GetTagManager(attributes);
        public void AddValue(double v) => Value += v;
        public override string ToString() => $"{Name}: {Value}";
    }

    public static class StateTagManagerExtension
    {
        public static IEnumerable<IValueTag> ConcatTags(this ITagManager mgr, IEnumerable<IValueTag> tags)=> mgr.Tags.Concat(tags);
        public static IEnumerable<IValueTag> ConcatTags(this ITagManager mgr, ITagManager other) => mgr.ConcatTags(other.Tags);
        public static IEnumerable<IValueTag> ConcatTags(this ITagManager mgr, params ITagManager[] others) => mgr.Tags.Concat(others.SelectMany(o=>o.Tags));
    }
}