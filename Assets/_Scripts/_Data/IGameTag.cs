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
}