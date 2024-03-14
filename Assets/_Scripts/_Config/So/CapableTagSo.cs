using System.Linq;
using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "CapableTagSo", menuName = "配置/标签/角色/公式/能力")]
    public class CapableTagSo : FormulaTagSo
    {
        [SerializeField] private TagFactor[] _fields;
        protected TagFactor[] Factors => _fields;
        public override TagType TagType => TagType.Capable;
        public override double GetValue(IRoleData role) => _fields.Sum(f => f.GetValue(role));
    }
}