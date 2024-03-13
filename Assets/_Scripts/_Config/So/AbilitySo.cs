using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "AbilitySo", menuName = "配置/标签/角色/属性")]
    public class AbilitySo : RoleTagSoBase
    {
        public string CName;
        public override TagType TagType => TagType.Ability;
    }
}