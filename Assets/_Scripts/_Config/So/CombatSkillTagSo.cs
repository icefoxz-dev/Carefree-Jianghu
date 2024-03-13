using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "CombatSkillTagSo", menuName = "配置/标签/技能/武功")]
    public class CombatSkillTagSo : SkillTagSo, ICombatSkill
    {
        [SerializeField] private BasicSkillTagSo _basic;
        [SerializeField] private SkillLevelMap _map;
        public override SkillType SkillType => SkillType.Combat;
        public override double GetPower(int level, IRoleData role) => _map.GetValue(level);
        public IBasicSkill Basic => _basic;
    }
}