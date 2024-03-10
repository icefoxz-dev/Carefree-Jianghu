using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "TraitSo", menuName = "配置/标签/角色/特征")]
    public class TraitSo : RoleTagSoBase
    {
        public override ITagManager GetTagManager(IRoleAttributes attributes) => attributes.Trait;
    }
}