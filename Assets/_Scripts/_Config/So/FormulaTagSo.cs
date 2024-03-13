using _Data;

namespace _Config.So
{
    public abstract class FormulaTagSo : RoleTagSoBase, IFormulaTag
    {
        public abstract double GetValue(IRoleData role);
    }
}