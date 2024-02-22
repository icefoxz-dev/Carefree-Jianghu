using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace _Data
{
    /// <summary>
    /// 标签管理类
    /// </summary>
    public class TagManager : ITagManager
    {
        private List<FuncTag> _tags;

        public TagManager(ITagManager tagManager)
        {
            _tags = tagManager.Tags.Select(t => new FuncTag(t)).ToList();
        }
        public TagManager(IEnumerable<IFuncTag> tags)
        {
            _tags = tags.Select(t => new FuncTag(t)).ToList();
        }

        public IReadOnlyList<IFuncTag> Tags => _tags;

        public void AddTag(IFuncTag tag)
        {
            var t = GetFirstOrDefault(tag);
            if (t != null) throw new DuplicateNameException($"tag.{tag.Name} already exist!");
            _tags.Add(new FuncTag(tag));
        }

        public void RemoveTag(IFuncTag tag)
        {
            var t = GetFirstOrDefault(tag);
            _tags.Remove(t);
        }

        private FuncTag GetFirstOrDefault(IFuncTag tag) => _tags.FirstOrDefault(t => t.Name == tag.Name);

        public void AddTagValue(IFuncTag tag)
        {
            var t = GetFirstOrDefault(tag);
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
    public record FuncTag : IFuncTag
    {
        public IGameTag GameTag { get; }

        public string Name => GameTag.Name;
        public double Value { get; private set; }

        public FuncTag(IFuncTag tag)
        {
            GameTag = tag.GameTag;
            Value = tag.Value;
        }

        public FuncTag(IGameTag tag, double value = 0)
        {
            GameTag = tag;
            Value = value;
        }

        public void SetPlayer(IRoleData player)
        {
            var manager = GameTag.GetTagManager(player.Prop);
            manager.AddTagValue(this);
        }

        public ITagManager GetTagManager(IRoleProperty property) => GameTag.GetTagManager(property);
        public void AddValue(double v) => Value += v;
    }
}