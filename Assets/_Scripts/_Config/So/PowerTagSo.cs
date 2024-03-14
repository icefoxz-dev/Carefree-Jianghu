using System;
using System.Linq;
using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "PowerTagSo", menuName = "配置/标签/角色/公式/战力")]
    public class PowerTagSo : CapableTagSo
    {
        public override double GetValue(IRoleData role)
        {
            var skill = role.Attributes.Skill;
            var fieldsValue = Factors.Sum(f => f.GetValue(role));
            var basicValue = skill.BasicSkills.Sum(s => s.Tag.GetPower(s.Level, role));
            var combatPower = role.EquipSet.Combat?.GetPower(role) ?? 0;
            var forcePower = role.EquipSet.Force?.GetPower(role) ?? 0;
            var dodgePower = role.EquipSet.Dodge?.GetPower(role) ?? 0;
            return fieldsValue + basicValue + combatPower + forcePower + dodgePower;
        }
    }
}