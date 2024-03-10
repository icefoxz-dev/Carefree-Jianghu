using _Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "FuncTagSo", menuName = "配置/标签/功能")]
    public class ValueTagSo : AutoNameSoBase, IValueTag
    {
        [SerializeField,FormerlySerializedAs("_gameTag")] private RoleTagSoBase roleTag;
        [SerializeField] private double _value = 1;

        public IRoleTag Tag => roleTag;

        public double Value => _value;
        public ITagManager GetTagManager(IRoleAttributes attributes) => roleTag.GetTagManager(attributes);
    }
}