using System.Linq;
using System.Text;
using _Data;
using Debug = UnityEngine.Debug;

namespace _Game._Models
{
    public class GameWorld : ModelBase
    {
        public EpisodeBase CurrentEp { get; private set; }
        public Character[] Team { get; private set; }
        public OccasionModel[] Occasions { get; private set; }

        public PlayerData Player { get; private set; }//玩家数据
        public OccasionModel CurrentOccasion { get; private set; }//当前场景
        public WorldInfo Info { get; private set; } //世界信息

        public void Init()
        {
            SendEvent(GameEvent.Game_Start);
            TestInit();
        }

        private void TestInit()
        {
            Occasions = Game.Config.ActivityCfg.GetOccasions().Select(o => new OccasionModel(o)).ToArray();
            //;new[]
            //{
            //    new OccasionModel("睡觉",
            //        new IRolePlacing[] { new RolePlacing(RolePlacing.Index.Solo, RolePlacing.Modes.Team, null) })
            //};
            CurrentEp = new TestEpisode(Game.Config.GetEpisode(0));
            Player = new PlayerData(Game.Config.GetPresetPlayer(), Game.Config.CharacterAttributeMap);
            Team = Game.Config.GetCharacters().Select(r=>new Character(r)).ToArray();
            Info = new WorldInfo();
            
            DebugInfo(Player);
            SendEvent(GameEvent.Episode_Start);
        }

        public void DebugInfo(PlayerData player)
        {
            var sb = new StringBuilder();
            sb.Append($"玩家：{player}\n武[{player.Power}]\n学[{player.Wisdom}]\n力[{player.Strength}]\n智[{player.Intelligent}]\n银[{player.Silver}]\n体[{player.Stamina}]");
            sb.Append(TagLog(player.Capable,"属性"));
            sb.Append(TagLog(player.Skill,"技能"));
            Debug.Log(sb);
            return;

            string TagLog(ITagManager tm, string tagName)
            {
                var s = new StringBuilder();
                foreach (var tag in tm.Tags)
                    s.Append($"\n{tagName}： {tag.Name}: {tag.Value}");
                return s.ToString();
            }
        }

        public void SetCurrentOccasion(OccasionModel occasion)
        {
            CurrentOccasion = occasion;
            SendEvent(GameEvent.Occasion_Update);
        }

        public void NextRound()
        {
            foreach (var tag in CurrentOccasion.Results)
                tag.SetPlayer(Player);
            Info.NextRound();
            SendEvent(GameEvent.Player_Update);
        }
    }
}