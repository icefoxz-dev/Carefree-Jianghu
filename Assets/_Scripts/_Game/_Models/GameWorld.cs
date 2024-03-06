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
            CurrentEp = new TestEpisode(Game.Config.GetEpisode(0));
            Player = new PlayerData(Game.Config.GetPresetPlayer(), Game.Config.CharacterAttributeMap);
            Team = Game.Config.GetCharacters().Select(r=>new Character(r)).ToArray();
            Info = new WorldInfo();
            
            UpdateRound();
            DebugInfo(Player);
            SendEvent(GameEvent.Episode_Start);
        }

        private void UpdateRound()
        {
            var l = Game.Config.ActivityCfg.GetOccasions()
                .Where(o => o.GetExcludedTerms(Player).Length == 0).ToArray();
            Occasions = l
                .Select(o => new OccasionModel(o)).ToArray();
            SendEvent(GameEvent.Occasion_Update);
        }

        public void DebugInfo(PlayerData player)
        {
            var sb = new StringBuilder();
            sb.Append($"玩家：{player}\n武[{player.Power}]\n学[{player.Wisdom}]\n力[{player.Strength}]\n智[{player.Intelligent}]\n银[{player.Silver}]\n体[{player.Stamina}]");
            sb.Append(TagLog(player.Capable,"属性"));
            sb.Append(TagLog(player.Status,"状态"));
            sb.Append(TagLog(player.Skill,"技能"));
            sb.Append(TagLog(player.Inventory, "物品"));
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
            UpdateRound();
        }

        /// <summary>
        /// 尝试进行下个回合，返回不过的条件
        /// </summary>
        /// <returns></returns>
        public IPlotTerm[] TryProceedRound()
        {
            var notInTerms = CurrentOccasion.GetExcludedTerms(Player);
            if (notInTerms.Any())
                return notInTerms;
            CurrentOccasion.UpdateRole(Player);
            Info.NextRound();
            UpdateRound();
            return notInTerms;
        }
    }
}