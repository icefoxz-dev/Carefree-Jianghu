namespace _Data
{
    public interface IRoleEquipSet
    {
        ISkillSet<ICombatSkill> Combat { get; }
        ISkillSet<ISkillTag> Force { get; }
        ISkillSet<ISkillTag> Dodge { get; }
    }
}