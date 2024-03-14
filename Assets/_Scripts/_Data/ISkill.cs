namespace _Data
{
    public enum SkillType
    {
        /// <summary>
        /// 内功-战力加成
        /// </summary>
        Force,
        /// <summary>
        /// 武功-战力加成
        /// </summary>
        Combat,
        /// <summary>
        /// 轻功-战力加成
        /// </summary>
        Dodge,
        /// <summary>
        /// 基础-仅用武功升级的判断依据和战力加成
        /// </summary>
        Basic
    }
    /// <summary>
    /// 技能
    /// </summary>
    public interface ISkillTag : IGameTag
    {
        SkillType SkillType { get; }
        int MaxLevel { get; }
        double GetPower(int level, IRoleData role);
    }

    public interface IBasicSkill : ISkillTag
    {

    }

    public interface ICombatSkill : ISkillTag
    {
        IBasicSkill Basic { get; }
    }

    public interface ISkillSet<out T> where T : ISkillTag
    {
        T Tag { get; }
        int Level { get; }
        double GetPower(IRoleData role);
    }
}