using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Data
{
    /// <summary>
    /// 功能标签，代表游戏中的一个功能标签。每个标签都有一个名称，表示玩家在游戏中的选择和行动。
    /// </summary>
    public interface IFuncTag : IPlotTag
    {
        void SetPlayer(IPlayerData player);
    }
    
    /// <summary>
    /// 剧情标签, 代表故事中的一个交互标签。每个标签都有一个名称和一个数值，表示玩家在故事中的选择和行动。
    /// </summary>
    public interface IPlotTag: IGameTag
    {
        double Value { get; }
    }

    /// <summary>
    /// 剧情交互标签的组合
    /// </summary>
    public interface IPlotTerm : IPlotTag
    {
        PlotTagClause Clause { get; }
        bool IsInTerm(IPlotTag other);
    }

    public enum PlotTagClause
    {
        [InspectorName("有标签")] HasTag,
        [InspectorName("相等")] Equal,
        [InspectorName("大相等于")] Exceed,
        [InspectorName("小于")] Less,
    }
    public record PlotTag (IPlotTag Tag) : IPlotTag
    {
        private IPlotTag Tag { get; } = Tag;
        public string Name { get; } = Tag.Name;
        public double Value { get; } = Tag.Value;
        public bool IsInTerm(IPlotTag other, PlotTagClause clause) => PlotTagExtension.IsInTerm(this, other, clause);
        public ITagManager GetTagManager(IPlayerProperty property) => Tag.GetTagManager(property);
    }

    public static class PlotTagExtension
    {
        public static bool IsInTerm(this IPlotTag tag, IPlotTag other, PlotTagClause clause)
        {
            if (tag.Name != other.Name) return false;
            return clause switch
            {
                PlotTagClause.HasTag => true,
                PlotTagClause.Equal => Math.Abs(tag.Value - other.Value) < 0.001,
                PlotTagClause.Exceed => tag.Value >= other.Value,
                PlotTagClause.Less => tag.Value < other.Value,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static bool IsInTerm(this IEnumerable<IPlotTerm> terms, IEnumerable<IPlotTag> tags) =>
            terms.All(te => tags.Any(te.IsInTerm));
    }
}