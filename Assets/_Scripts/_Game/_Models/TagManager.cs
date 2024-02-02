using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using _Data;

namespace _Game._Models
{
    /// <summary>
    /// 标签管理类
    /// </summary>
    public class TagManager : ITagManager
    {
        private List<Tag> tags;
        public IReadOnlyList<IFuncTag> Tags => tags;

        public void AddTag(IFuncTag tag)
        {
            var t = GetFirstOrDefault(tag);
            if (t != null) throw new DuplicateNameException($"tag.{tag.Name} already exist!");
            tags.Add(new Tag(tag));
        }

        public void RemoveTag(IFuncTag tag)
        {
            var t = GetFirstOrDefault(tag);
            tags.Remove(t);
        }

        private Tag GetFirstOrDefault(IFuncTag tag) => tags.FirstOrDefault(t => t.Name == tag.Name);

        public void AddTagValue(IFuncTag tag)
        {
            var t = GetFirstOrDefault(tag);
            t.AddValue(tag.Value);
        }

        //标签状态类，Value会变化
        private class Tag : IFuncTag
        {
            public IFuncTag Func { get; }
            public string Name => Func.Name;
            private double value;
            public double Value => value;
            public Tag(IFuncTag func) => Func = func;

            public void SetPlayer(IPlayerData player) => Func.SetPlayer(player);
            public ITagManager GetTagManager(IPlayerProperty property) => Func.GetTagManager(property);
            public void AddValue(double v) => value += v;
        }
    }
}