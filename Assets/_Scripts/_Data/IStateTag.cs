using System;

namespace _Data
{
    /// <summary>
    /// 状态标签，代表游戏中的一个状态标签。每个状态标签都有一个名称，表示玩家在游戏中的状态。
    /// </summary>
    public interface IStatusTag : IValueTag
    {
        double Max { get; }
        double Min { get; }
        void SetMax();
        void SetMin();
    }

    /// <summary>
    /// 状态标签
    /// </summary>
    /// <param name="GameTag"></param>
    /// <param name="Value"></param>
    /// <param name="Max"></param>
    /// <param name="Min"></param>
    public record StatusTag(IGameTag GameTag, double Value, double Max, double Min = 0)
        : IStatusTag
    {
        public string Name => GameTag.Name;
        public ITagManager GetTagManager(IRoleAttributes attributes) => GameTag.GetTagManager(attributes);

        public double Value { get; private set; } = Value;
        public double Max { get; } = Max;
        public double Min { get; } = Min;

        /// <summary></summary>
        public IGameTag GameTag { get; } = GameTag;
        public void SetMax() => Value = Max;
        public void SetMin() => Value = Min;

        public StatusTag(IValueTag tag, double max, double min = 0) : this(tag.GameTag, tag.Value, max, min)
        {
        }

        public void Add(double value) => Value = Math.Clamp(Value + value, Min, Max);
        public override string ToString()=> $"{Name}:[{Value}/{Max}]";
    }

    public static class StateTagExtension
    {
        public static StatusTag ToStatusTag(this IGameTag tag, double value, double max, double min = 0) => new(tag, value, max, min);
        public static StatusTag ToStatusTag(this IStatusTag tag) => new(tag.GameTag, tag.Value, tag.Max, tag.Min);
        public static StatusTag ToStatusTag(this IValueTag tag, double max, double min = 0) => new(tag, max, min);
    }
}