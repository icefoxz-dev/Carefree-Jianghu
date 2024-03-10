using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "BasicSkillTagSo", menuName = "配置/标签/技能/基本功")]
    public class BasicSkillTagSo: SkillTagSo
    {
        
    }

    public class SkillTagSo: RoleTagSoBase
    {
        public override ITagManager GetTagManager(IRoleAttributes attributes) => attributes.Skill;
    }
}