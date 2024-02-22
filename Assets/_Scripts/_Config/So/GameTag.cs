using _Data;

namespace _Config.So
{
    public abstract class GameTagSoBase : AutoNameSoBase, IGameTag
    {
        public abstract ITagManager GetTagManager(IRoleProperty property);
    }
}