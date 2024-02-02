using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "CapableSo", menuName = "配置/标签/角色/能力")]
    public class CapableSo : GameTag
    {
        public string CName;
        public override ITagManager GetTagManager(IPlayerProperty property) => property.Capable;
    }
}