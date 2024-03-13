using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "StoryTagSo", menuName = "配置/标签/剧情/故事")]
    public class StoryTagSo: RoleTagSoBase
    {
        public override TagType TagType => TagType.Story;
    }
}