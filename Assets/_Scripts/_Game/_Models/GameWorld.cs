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
            Player = new PlayerData(Game.Config.GetPresetPlayer(), Game.Config.CharacterAttributeMap);
            Team = Game.Config.GetCharacters().Select(r=>new Character(r)).ToArray();
            
            DebugInfo(Player);
            SendEvent(GameEvent.Episode_Start);
        }

        private void DebugInfo(PlayerData player)
        {
            Debug.Log($"玩家：{player}\n武[{player.Power}]\n学[{player.Wisdom}]\n力[{player.Strength}]\n智[{player.Intelligent}]\n银[{player.Silver}]\n体[{player.Stamina}]");
            TagLog(player.Capable,"属性");
            TagLog(player.Skill,"技能");
            return;

            void TagLog(ITagManager tm, string tagName)
            {
                foreach (var tag in tm.Tags)
                    Debug.Log($"{tagName}： {tag.Name}: {tag.Value}");
            }
        }

        public void SetCurrentOccasion(IOccasion occasion)
        {
            CurrentOccasion = occasion;
            SendEvent(GameEvent.Occasion_Update);
        }
    }
}