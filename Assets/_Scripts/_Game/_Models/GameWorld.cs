using System.Diagnostics;
using System.Linq;
using _Data;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace _Game._Models
{
    public interface IGameWorld
    {
        ICharacter[] Team { get; }
        EpisodeBase CurrentEp { get; }
        PlayerData Player { get; }
        IOccasion[] Occasions { get; }
        IOccasion CurrentOccasion { get; }
    }
    
    public class GameWorld : ModelBase, IGameWorld
    {
        public EpisodeBase CurrentEp { get; private set; }
        public Character[] Team { get; private set; }
        public IOccasion[] Occasions { get; private set; }

        public PlayerData Player { get; private set; }//玩家数据
        public IOccasion CurrentOccasion { get; private set; }//当前场景

        ICharacter[] IGameWorld.Team => Team;

        public void Init()
        {
            SendEvent(GameEvent.Game_Start);
            TestInit();
        }

        private void TestInit()
        {
            Occasions = new IOccasion[]
            {
                new TestOccasion("睡觉",
                    new IRolePlacing[] { new RolePlacing(RolePlacing.Index.Solo, RolePlacing.Modes.Team, null) })
            };
            CurrentEp = new TestEpisode(Game.Config.GetEpisode(0));
            Team = Game.Config.GetCharacters().Select(r=>new Character(r)).ToArray();
            foreach (var (index, frame) in CurrentEp.FrameMap) 
                Debug.Log($"{index} --- {frame.Name}");
            SendEvent(GameEvent.Episode_Start);
        }

        public void SetCurrentOccasion(IOccasion occasion)
        {
            CurrentOccasion = occasion;
            SendEvent(GameEvent.Occasion_Update);
        }
    }
}