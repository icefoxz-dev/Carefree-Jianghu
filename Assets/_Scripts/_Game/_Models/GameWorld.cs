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
    }
    
    public class GameWorld : ModelBase, IGameWorld
    {
        public EpisodeBase CurrentEp { get; private set; }
        public Character[] Team { get; private set; }
        public PlayerData Player { get; private set; }

        ICharacter[] IGameWorld.Team => Team;

        public void Init()
        {
            SendEvent(GameEvent.Game_Start);
            TestInit();
        }

        private void TestInit()
        {
            CurrentEp = new TestEpisode(Game.Config.GetEpisode(0));
            Team = Game.Config.GetCharacters().Select(r=>new Character(r)).ToArray();
            foreach (var (index, frame) in CurrentEp.FrameMap) 
                Debug.Log($"{index} --- {frame.Name}");
            SendEvent(GameEvent.Episode_Start);
        }
    }
}