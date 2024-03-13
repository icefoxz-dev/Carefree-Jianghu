using _Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "FuncTagSo", menuName = "配置/标签/功能")]
    public class ValueTagSo : AutoNameSoBase, IValueTag
    {
        [SerializeField,FormerlySerializedAs("_roleTag")] private GameTagSoBase _gameTag;
        [SerializeField] private double _value = 1;

        public IGameTag Tag => _gameTag;

        public double Value => _value;
        public TagType TagType => _gameTag.TagType;
    }
}