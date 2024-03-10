using _Data;

namespace _Config.So
{
    public abstract class GameTagSoBase : AutoNameSoBase, IGameTag
    {
    }

    public abstract class RoleTagSoBase : GameTagSoBase, IRoleTag
    {
        public abstract ITagManager GetTagManager(IRoleAttributes attributes);
    }
}