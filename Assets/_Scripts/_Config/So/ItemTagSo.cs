using System;
using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "ItemTagSo", menuName = "配置/标签/物品")]
    public class ItemTagSo : FuncTagSoBase
    {
        [SerializeField] private TagField[] _tags;

        public override void UpdateRole(IRoleData role)
        {
            foreach (var tag in _tags) tag.Proceed(role);
        }
        public override ITagManager GetTagManager(IRoleAttributes attributes) => attributes.Inventory;//物品引用
     
        [Serializable]
        private class TagField
        {
            [SerializeField] private GameTagSoBase _tag;
            [SerializeField] private double _value;

            public void Proceed(IRoleData role)
            {
                var tm = _tag.GetTagManager(role.Attributes);//属性引用
                tm.AddTagValue(new ValueTag(_tag, _value)); //给属性赋值
            }
        }
    }


    public abstract class FuncTagSoBase : GameTagSoBase, IFuncTag
    {
        public IGameTag GameTag => this;
        public abstract void UpdateRole(IRoleData role);
    }
}