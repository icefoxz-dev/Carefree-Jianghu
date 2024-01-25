using System.Linq;
using _Data;

namespace _Game._Models
{
    public interface IGameWorld
    {
        IRolePlay[] Team { get; }
        IEpisode CurrentEp { get; }
    }
    
    public class GameWorld : ModelBase, IGameWorld
    {
        public EpisodeBase CurrentEp { get; private set; }
        public RolePlay[] Team { get; private set; }

        IRolePlay[] IGameWorld.Team => Team;
        IEpisode IGameWorld.CurrentEp => CurrentEp;

        public void Init()
        {
            SendEvent(GameEvent.Game_Start);
            TestInit();
        }

        private void TestInit()
        {
            CurrentEp = new TestEpisode(Game.Config.GetEpisode(0));
            Team = Game.Config.GetRoles().Select(r=>new RolePlay(r)).ToArray();
            SendEvent(GameEvent.Episode_Start);
        }
    }
}