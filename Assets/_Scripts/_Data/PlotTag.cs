using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Data
{
    /// <summary>
    /// 剧情交互标签的组合
    /// </summary>
    public interface IPlotTerm : IValueTag
    {
        PlotTagClause Clause { get; }
        bool IsInTerm(IValueTag other);
        bool IsInTerm(IRoleAttributes role);
    }

    public enum PlotTagClause
    {
        [InspectorName("相等")] Equal,
        [InspectorName("大于")] Exceed,
        [InspectorName("小于")] Less,
    }
    public record PlotTag (IValueTag Tag) : IValueTag
    {
        private IValueTag Tag { get; } = Tag;
        public string Name { get; } = Tag.Name;
        public double Value { get; } = Tag.Value;
        public bool IsInTerm(IValueTag other, PlotTagClause clause) => PlotTagExtension.IsInTerm(this, other, clause);
        public ITagManager GetTagManager(IRoleAttributes attributes) => Tag.GetTagManager(attributes);
    }

    public static class PlotTagExtension
    {
        public static bool IsInTerm(this IValueTag tag, IValueTag other, PlotTagClause clause)
        {
            if (clause == PlotTagClause.Equal && tag.Value == 0) return true;//如果是相等条件，且值为0，则返回true(作为无标签的时候不会被名字判断给过滤了)
            if (tag.Name != other.Name) return false;
            return clause switch
            {
                PlotTagClause.Equal => tag.Value == 0 || Math.Abs(tag.Value - other.Value) < 0.001,
                PlotTagClause.Exceed => other.Value > tag.Value,
                PlotTagClause.Less => other.Value < tag.Value,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static bool IsInTerm(this IEnumerable<IPlotTerm> terms, IEnumerable<IValueTag> tags) =>
            terms.All(te => tags.Any(te.IsInTerm));

        public static bool IsInTerm(this IEnumerable<IPlotTerm> terms, IRoleAttributes role) => terms.IsInTerm(role.GetAllTags());
    }
}