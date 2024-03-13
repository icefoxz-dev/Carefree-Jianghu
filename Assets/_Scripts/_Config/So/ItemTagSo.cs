using System;
using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "ItemTagSo", menuName = "配置/标签/物品")]
    public class ItemTagSo : FuncTagSoBase
    {
        public override TagType TagType => TagType.Inventory;
        [SerializeField] private TagField[] _tags;

        public override void UpdateRole(IRoleData role)
        {
            foreach (var tag in _tags) role.Proceed(tag);
        }
        
     
        [Serializable]
        private class TagField : IValueTag
        {
            [SerializeField] private RoleTagSoBase _tag;
            [SerializeField] private double _value;
            public IGameTag Tag => _tag;

            public double Value => _value;
            public string Name => _tag.Name;
            public TagType TagType => _tag.TagType;
        }

    }

    public abstract class FuncTagSoBase : RoleTagSoBase, IFuncTag
    {
        public IRoleTag RoleTag => this;
        public abstract void UpdateRole(IRoleData role);
    }
}