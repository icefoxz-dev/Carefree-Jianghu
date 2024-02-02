using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "ChapterSo", menuName = "配置/标签/章节")]
    public class ChapterTagSo : GameTag
    {
        public override ITagManager GetTagManager(IPlayerProperty property) => property.ChapterTag;
    }
}