using _Data;

namespace _Config.So
{
    public abstract class GameTagSoBase : AutoNameSoBase, IGameTag
    {
        public abstract TagType TagType { get; }
    }

    public abstract class RoleTagSoBase : GameTagSoBase, IRoleTag
    {
    }
}