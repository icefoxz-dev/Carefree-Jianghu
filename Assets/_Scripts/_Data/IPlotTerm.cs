using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Data
{
    /// <summary>
    /// 剧情交互标签的组合
    /// </summary>
    public interface ITagTerm : IValueTag
    {
        TagClauses Clause { get; }
        bool IsInTerm(IValueTag other);
        bool IsInTerm(IRoleAttributes role);
    }

    public enum TagClauses
    {
        [InspectorName("相等|拥有")] Equal,
        [InspectorName("大于")] Exceed,
        [InspectorName("小于")] Less,
    }

    public static class PlotTagExtension
    {
        public static bool IsInTerm(this IValueTag tag, IValueTag other, TagClauses clause)
        {
            if (tag.Value == 0 && clause == TagClauses.Equal)
                throw new InvalidOperationException($"{clause}必须在列表判断中剔除了，因为检查的是是否有tag是无法直接调用IsInTerm来判断！");
            if (tag.Name != other.Name) return false;
            return clause switch
            {
                TagClauses.Equal => tag.Value == 0 || Math.Abs(tag.Value - other.Value) < 0.001,
                TagClauses.Exceed => other.Value > tag.Value,
                TagClauses.Less => other.Value < tag.Value,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static ITagTerm[] GetExcludedTerms(this IEnumerable<ITagTerm> terms, IEnumerable<IValueTag> tags)
        {
            var termList = terms.ToArray();
            var tagList = tags.ToArray();
            var haventTerms = termList.Where(t => t.Clause == TagClauses.Equal && t.Value == 0).ToArray(); //找出"没有tag"的条件
            var existTags = haventTerms.Where(term => tagList.Any(t => t.Name == term.Name)).ToArray();
            var notInTerms = termList.Except(haventTerms).Where(te => tagList.All(t => !te.IsInTerm(t))).ToArray(); //如果所有的条件都有tag，就根据IsInTerm()的结果来判断
            return existTags.Concat(notInTerms).ToArray();
        }

        private static bool IsInTerm(this IEnumerable<ITagTerm> terms, IEnumerable<IValueTag> tags) => terms.GetExcludedTerms(tags).Length == 0;

        public static bool IsInTerm(this IEnumerable<ITagTerm> terms, IRoleAttributes role) => terms.IsInTerm(role.GetAllTags());
        public static bool IsInTerm(this IEnumerable<ITagTerm> terms, IRoleData role) => terms.IsInTerm(role.Attributes.GetAllTags());
        public static ITagTerm[] GetExcludedTerms(this IEnumerable<ITagTerm> terms, IRoleData role) => terms.GetExcludedTerms(role.Attributes);
        private static ITagTerm[] GetExcludedTerms(this IEnumerable<ITagTerm> terms, IRoleAttributes attributes) => terms.GetExcludedTerms(attributes.GetAllTags());
    }
}