using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "ChapterSo", menuName = "配置/标签/章节")]
    public class ChapterTagSo : GameTagSoBase
    {
        public override ITagManager GetTagManager(IRoleProperty property) => property.ChapterTag;
    }
}