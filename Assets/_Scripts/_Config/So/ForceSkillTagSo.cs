using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "CombatSkillTagSo", menuName = "配置/标签/技能/内功")]
    public class ForceSkillTagSo : SkillTagSo
    {
        [SerializeField] private SkillLevelMap _map;
        public override SkillType SkillType => SkillType.Force;
        public override double GetPower(int level, IRoleData role) => _map.GetValue(level);
    }
}