using System;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace _Data
{
    /// <summary>
    /// 剧情交互的标签, 代表故事中的一个交互标签。每个标签都有一个名称和一个数值，表示玩家在故事中的选择和行动。
    /// </summary>
    public interface IPlotTag
    {
        string Name { get; }
        double Value { get; }
    }
    public enum PlotTagCompares
    {
        [InspectorName("有标签")] HasTag,
        [InspectorName("相等")] Equal,
        [InspectorName("大相等于")] Exceed,
        [InspectorName("小于")] Less,
    }
    public record PlotTag (double Value, string Name) : IPlotTag
    {
        public string Name { get; } = Name;
        public double Value { get; } = Value;
    }

    public static class PlotTagExtension
    {
        public static bool IsInTerm(this IPlotTag tag, IPlotTag other, PlotTagCompares compare)
        {
            if (tag.Name != other.Name) return false;
            return compare switch
            {
                PlotTagCompares.HasTag => true,
                PlotTagCompares.Equal => tag.Value == other.Value,
                PlotTagCompares.Exceed => tag.Value >= other.Value,
                PlotTagCompares.Less => tag.Value < other.Value,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}