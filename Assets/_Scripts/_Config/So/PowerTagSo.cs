using System;
using System.Linq;
using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "PowerTagSo", menuName = "配置/标签/角色/公式/武力")]
    public class PowerTagSo : FormulaTagSo
    {
        [SerializeField] private TagFactor[] _fields;
        public override TagType TagType => TagType.Capable;
        public override double GetValue(IRoleData role)
        {
            var skill = role.Attributes.Skill;
            var fieldsValue = _fields.Sum(f => f.GetValue(role));
            var basicValue = skill.BasicSkills.Sum(s => s.Tag.GetPower(s.Level, role));
            var combatPower = role.EquipSet.Combat.GetPower(role);
            var forcePower = role.EquipSet.Force.GetPower(role);
            var dodgePower = role.EquipSet.Dodge.GetPower(role);
            return fieldsValue + basicValue + combatPower + forcePower + dodgePower;
        }

        [Serializable] private class TagFactor
        {
            [SerializeField] private GameTagSoBase _tagSo;
            [SerializeField] private double _factor = 1;

            public double GetValue(IRoleData role) => role.Attributes.GetValue(_tagSo);
        }
    }
}