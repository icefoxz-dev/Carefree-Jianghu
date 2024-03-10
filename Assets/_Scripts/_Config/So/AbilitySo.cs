using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "CapableSo", menuName = "配置/标签/角色/能力")]
    public class CapableTagSo : RoleTagSoBase, ICapableTag
    {
        public IRoleTag Tag => this;

        public double Value => throw new System.NotImplementedException(); //todo 暂时无法直接给出公式，公式需要根据角色的能力来计算
        public override ITagManager GetTagManager(IRoleAttributes attributes) => attributes.Capable;
    }

    [CreateAssetMenu(fileName = "AbilitySo", menuName = "配置/标签/角色/属性")]
    public class AbilitySo : RoleTagSoBase
    {
        public string CName;
        public override ITagManager GetTagManager(IRoleAttributes attributes) => attributes.Ability;
    }
}