using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "EpisodeSo", menuName = "配置/标签/剧集")]
    public class EpisodeTagSo : GameTagSoBase
    {
        public override ITagManager GetTagManager(IRoleProperty property) => property.EpisodeTag;
    }
}