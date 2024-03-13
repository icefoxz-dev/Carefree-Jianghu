using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "StateTagSo", menuName = "配置/标签/角色/状态")]
    public class StatusTagSo : RoleTagSoBase
    {
        public string CName;
        [SerializeField] private double _value;
        [SerializeField] private double _max;
        [SerializeField] private double _min;
        public override TagType TagType => TagType.Status;
        public ITagStatus GetStatusTag() => this.ToStatusTag(_value, _max, _min);
    }
}