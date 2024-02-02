using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "TraitSo", menuName = "配置/标签/角色/特征")]
    public class TraitSo : GameTag
    {
        public override ITagManager GetTagManager(IPlayerProperty property) => property.Trait;
    }
}