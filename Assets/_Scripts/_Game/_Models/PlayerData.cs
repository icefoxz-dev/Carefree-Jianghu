using System;
using System.Collections.Generic;
using System.Linq;
using _Data;
using UnityEngine;

namespace _Game._Models
{
    /// <summary>
    /// 玩家信息，包括玩家的角色信息和玩家的游戏数据。
    /// </summary>
    public partial class RoleData 
    {
        ////这里的是一些属性参考代码，为了方便阅读，我把它们放在了一起
        public double Strength => this.Strength();
        public double Intelligent => this.Intelligent();
        public double Power => this.Power();
        public double Wisdom => this.Wisdom();
        public double Silver => this.Silver();
        public double Stamina => this.Stamina();

        public int Id => _character.Id;
        public string Name => _character.Name;
        public string Description => _character.Description;
        public GameObject Prefab => _character.Prefab;

        public override string ToString() => _character.ToString();
    }
    //下面是实现构造函数的代码
    public partial class RoleData : IRoleData, IRoleAttributes, ICharacter
    {
        private readonly Character _character;
        private readonly FormulaTagManager _capable;
        private readonly TagManager _trait;
        private readonly TagManager _ability;
        private readonly SkillTagManager _skill;
        private readonly StatusTagManager _status;
        private readonly TagManager _inventory;
        private readonly TagManager _story;
        private readonly EquipSet _equipSet;

        public RoleData(IRoleData playerData)
        {
            _character = new Character(playerData.Character);
            _capable = new FormulaTagManager(playerData.Attributes.Capable.FormulaTags, this);
            _trait = new TagManager(playerData.Attributes.Trait);
            _ability = new TagManager(playerData.Attributes.Ability);
            _skill = new SkillTagManager(playerData.Attributes.Skill.Skills.Select(s => (s, playerData.Attributes.Skill.GetTagValue(s))));
            _status = new StatusTagManager(playerData.Attributes.Status.Set.Cast<ITagStatus>());
            _inventory = new TagManager(playerData.Attributes.Inventory);
            _story = new TagManager(playerData.Attributes.Story);
            _equipSet = new EquipSet(playerData.EquipSet.Combat, playerData.EquipSet.Force, playerData.EquipSet.Dodge);
        }

        public ITagManager<IGameTag> Trait => _trait;
        public ITagManager<IGameTag> Ability => _ability;
        public ITagManager<IGameTag> Status => _status;
        public ISkillTagManager Skill => _skill;
        public ITagManager<IGameTag> Inventory => _inventory;
        public ITagManager<IGameTag> Story => _story;
        public IRoleEquipSet EquipSet => _equipSet;

        public IFormulaTagManager Capable => _capable;
        public ICharacter Character => _character;
        public void Proceed(IGameTag tag, double value)
        {
            switch (tag.TagType)
            {
                case TagType.Trait:
                    _trait.UpdateTag(tag, value);
                    break;
                case TagType.Ability:
                    _ability.UpdateTag(tag, value);
                    break;
                case TagType.Status:
                    _status.UpdateTag(tag, value);
                    break;
                case TagType.Skill:
                    _skill.UpdateTag((ISkillTag)tag, value);
                    break;
                case TagType.Inventory:
                    _inventory.UpdateTag(tag, value);
                    break;
                case TagType.Story:
                    _story.UpdateTag(tag, value);
                    break;
                case TagType.Capable:
                    throw new InvalidOperationException($"{tag}是公式标签，不可以赋值！");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        IRoleAttributes IRoleData.Attributes => this;
    }

    /// <summary>
    /// 装备 - 技能，物品等
    /// </summary>
    public class EquipSet : IRoleEquipSet
    {
        public ISkillSet<ICombatSkill> Combat { get; private set; }
        public ISkillSet<ISkillTag> Force { get; private set; }
        public ISkillSet<ISkillTag> Dodge { get; private set; }

        public EquipSet()
        {
            
        }
        public EquipSet(ISkillSet<ICombatSkill> combat, ISkillSet<ISkillTag> force, ISkillSet<ISkillTag> dodge)
        {
            Combat = combat;
            Force = force;
            Dodge = dodge;
        }
        public void SetForce(ISkillSet<ISkillTag> force) => Force = force;
        public void SetDodge(ISkillSet<ISkillTag> dodge) => Dodge = dodge;
        public void SetCombat(ISkillSet<ICombatSkill> combat) => Combat = combat;

    }

    public static class RoleDataExtension
    {
        public static double Strength(this IRoleData role) => Game.CharacterTags.GetStrength(role);
        public static double Intelligent(this IRoleData role) => Game.CharacterTags.GetIntelligent(role);
        public static double Power(this IRoleData role) => Game.CharacterTags.GetPower(role);
        public static double Wisdom(this IRoleData role) => Game.CharacterTags.GetWisdom(role);
        public static double Silver(this IRoleData role) => Game.CharacterTags.GetSilver(role);
        public static double Stamina(this IRoleData role) => Game.CharacterTags.GetStamina(role);
    }
}