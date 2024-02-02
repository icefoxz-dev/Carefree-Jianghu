using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "SkillTagSo", menuName = "配置/标签/角色/技能")]
    public class SkillTagSo: GameTag
    {
        public override ITagManager GetTagManager(IPlayerProperty property) => property.Skill;
    }
}