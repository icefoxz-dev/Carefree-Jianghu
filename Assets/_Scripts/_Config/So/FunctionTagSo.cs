using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "FuncTagSo", menuName = "配置/标签/功能")]
    public class FunctionTagSo: GameTag
    {
        public override ITagManager GetTagManager(IPlayerProperty property) => property.FuncTag;
    }
}