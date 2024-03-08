using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "EpisodeTagSo", menuName = "配置/标签/剧情/剧集")]
    public class EpisodeTagSo: GameTagSoBase
    {
        public override ITagManager GetTagManager(IRoleAttributes attributes) => attributes.Story;
    }
}