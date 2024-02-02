using _Data;

namespace _Config.So
{
    public abstract class GameTag : AutoNameSoBase, IGameTag
    {
        public abstract ITagManager GetTagManager(IPlayerProperty property);
    }
}