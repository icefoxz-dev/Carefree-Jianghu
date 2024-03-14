using _Data;
using System;
using UnityEngine;

namespace _Config.So
{
    public abstract class FormulaTagSo : RoleTagSoBase, IFormulaTag
    {
        public abstract double GetValue(IRoleData role);

        [Serializable] protected class TagFactor
        {
            [SerializeField] private GameTagSoBase _tagSo;
            [SerializeField] private double _factor = 1;

            public double GetValue(IRoleData role) => role.Attributes.GetValue(_tagSo);
        }
    }
}