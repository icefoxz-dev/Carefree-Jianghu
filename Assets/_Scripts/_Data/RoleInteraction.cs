using System.Collections.Generic;

namespace _Data
{
    /// <summary>
    /// 标签集合，用于获取值标签
    /// </summary>
    public interface ITagSet
    {
        IEnumerable<ITagValue> Set { get; }
    }
    /// <summary>
    /// 标签集
    /// </summary>
    public interface ITagSet<in T> : ITagSet where T : IGameTag
    {
        double GetTagValue(T tag);
    }

    /// <summary>
    /// 标签管理器
    /// </summary>
    public interface ITagManager<in T> : ITagSet<T> where T : IGameTag
    {
        void UpdateTag(T tag, double value);
    }

    public interface IFormulaTagManager : ITagSet<IFormulaTag>,ITagSet<IGameTag>
    {
        IEnumerable<IFormulaTag> FormulaTags { get; }
    }

    public interface ISkillTagManager : ITagSet<IGameTag>, ITagManager<ISkillTag>
    {
        IEnumerable<ISkillTag> Skills { get; }
        IEnumerable<ISkillSet<ICombatSkill>> CombatSkills { get; }
        IEnumerable<ISkillSet<IBasicSkill>> BasicSkills { get; }
        IEnumerable<ISkillSet<ISkillTag>> ForceSkills { get; }
        IEnumerable<ISkillSet<ISkillTag>> DodgeSkills { get; }
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
        IEnumerable<IFormulaTag> GetCapableTags { get; }
    }
}