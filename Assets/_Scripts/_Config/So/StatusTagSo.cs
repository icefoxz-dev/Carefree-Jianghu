using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "StateTagSo", menuName = "配置/标签/角色/状态")]
    public class StatusTagSo : GameTagSoBase
    {
        public string CName;
        [SerializeField] private double _value;
        [SerializeField] private double _max;
        [SerializeField] private double _min;
        public override ITagManager GetTagManager(IRoleAttributes attributes) => attributes.Status;
        public IStatusTag GetStatusTag() => this.ToStatusTag(_value, _max, _min);
    }
}