using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "CombatSkillTagSo", menuName = "配置/标签/技能/轻功")]
    public class DodgeSkillTagSo: SkillTagSo
    {
        [SerializeField] private SkillLevelMap _map;
        public override SkillType SkillType => SkillType.Dodge;
        public override double GetPower(int level, IRoleData role) => _map.GetValue(level);
    }
}