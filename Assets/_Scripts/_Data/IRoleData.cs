using System;
using System.Collections.Generic;
using System.Linq;

namespace _Data
{
    /// <summary>
    /// 玩家角色数据，包含玩家的角色信息和故事进程。
    /// </summary>
    public interface IRoleData
    {
        IRoleAttributes Attributes { get; }
        IRoleEquipSet EquipSet { get; }
        ICharacter Character { get; }
        void Proceed(IGameTag tag, double value);
    }

    /// <summary>
    /// 角色属性
    /// </summary>
    public interface IRoleAttributes
    {
        IFormulaTagManager Capable { get; }
        ITagManager<IGameTag> Trait { get; }
        ITagManager<IGameTag> Ability { get; }
        ITagManager<IGameTag> Status { get; }
        ISkillTagManager Skill { get; }
        ITagManager<IGameTag> Inventory { get; }
        ITagManager<IGameTag> Story { get; }

        IEnumerable<ITagValue> GetAllTags() => Capable.Set
            .Concat(Trait.Set).Concat(Ability.Set).Concat(Status.Set)
            .Concat(Skill.Set).Concat(Inventory.Set).Concat(Story.Set);
    }

    public static class RoleDataExtension
    {
        public static void Proceed(this IRoleData role, ITagValue val) => role.Proceed(val.Tag, val.Value);

        public static double GetValue(this IRoleAttributes role, IGameTag tag) => tag.TagType switch
        {
            TagType.Trait => role.Trait.GetTagValue(tag),
            TagType.Ability => role.Ability.GetTagValue(tag),
            TagType.Status => role.Status.GetTagValue(tag),
            TagType.Skill => role.Skill.GetTagValue(tag),
            TagType.Inventory => role.Inventory.GetTagValue(tag),
            TagType.Story => role.Story.GetTagValue(tag),
            TagType.Capable => role.Capable.GetTagValue(tag),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}