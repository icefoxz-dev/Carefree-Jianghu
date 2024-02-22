using System.Collections.Generic;

namespace _Data
{
    /// <summary>
    /// 角色属性
    /// </summary>
    public interface IRoleProperty
    {
        ITagManager Trait { get; }
        ITagManager Capable { get; }
        ITagManager Skill { get; }
        ITagManager EpisodeTag { get; }
        ITagManager ChapterTag { get; }
    }

    /// <summary>
    /// 玩家角色数据，包含玩家的角色信息和故事进程。
    /// </summary>
    public interface IRoleData
    {
        IRoleProperty Prop { get; }
        ICharacter Character { get; }
    }
    public interface ITagManager
    {
        IReadOnlyList<IFuncTag> Tags { get; }
        void AddTag(IFuncTag tag);
        void RemoveTag(IFuncTag tag);
        void AddTagValue(IFuncTag tag);
        double GetTagValue(IGameTag tag, bool throwErrorIfNoTag = false);
    }

    public interface ICharacterAttributeMap
    {
        double GetStrength(IRoleData player);
        double GetIntelligent(IRoleData player);
        double GetPower(IRoleData player);
        double GetWisdom(IRoleData player);
        double GetSilver(IRoleData player);
        double GetStamina(IRoleData player);
    }
}