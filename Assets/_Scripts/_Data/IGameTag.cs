using System;

namespace _Data
{
    public enum TagType
    {
        /// <summary>
        /// 性格
        /// </summary>
        Trait,
        /// <summary>
        /// 属性(非公式类型)
        /// </summary>
        Ability,
        /// <summary>
        /// 状态
        /// </summary>
        Status,
        /// <summary>
        /// 技能
        /// </summary>
        Skill,
        /// <summary>
        /// 物品
        /// </summary>
        Inventory,
        /// <summary>
        /// 剧情
        /// </summary>
        Story,
        /// <summary>
        /// 公式能力(战力等...)
        /// </summary>
        Capable
    }
    /// <summary>
    /// 游戏标签
    /// </summary>
    public interface IGameTag
    {
        string Name { get; }
        TagType TagType { get; }
    }

    public static class GameTagExtension
    {
        public static bool IsType(this IGameTag tag, TagType type) => tag.TagType == type;
        public static bool Is(this IGameTag tag, IGameTag other) =>
            other != null && tag.Name == other.Name && tag.TagType == other.TagType;
    }
}