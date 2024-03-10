using System.Collections.Generic;

namespace _Data
{
    /// <summary>
    /// 角色属性
    /// </summary>
    public interface IRoleAttributes
    {
        ITagManager Capable { get; }
        ITagManager Trait { get; }
        ITagManager Ability { get; }
        ITagManager Status { get; }
        ITagManager Skill { get; }
        ITagManager Inventory { get; }
        ITagManager Story { get; }
        IEnumerable<IValueTag> GetAllTags() => Capable.ConcatTags(Trait, Ability, Status, Skill, Inventory, Story);
    }

    /// <summary>
    /// 玩家角色数据，包含玩家的角色信息和故事进程。
    /// </summary>
    public interface IRoleData
    {
        IRoleAttributes Attributes { get; }
        ICharacter Character { get; }
    }
    public interface ITagManager
    {
        IEnumerable<IValueTag> Tags { get; }
        double GetTagValue(IGameTag tag, bool throwErrorIfNoTag = false);
        void AddTagValue(IValueTag tag);
    }

    public interface ICharacterTagsMap
    {
        double GetStrength(IRoleData player);
        double GetIntelligent(IRoleData player);
        double GetPower(IRoleData player);
        double GetWisdom(IRoleData player);
        double GetSilver(IRoleData player);
        double GetStamina(IRoleData player);
        bool IsGameOver(IRoleData player);
        IEnumerable<ICapableTag> GetCapableTags { get; }
    }
}