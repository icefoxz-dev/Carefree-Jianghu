using System.Linq;
using System.Text;
using _Data;
using Debug = UnityEngine.Debug;

namespace _Game._Models
{
    public class GameWorld : ModelBase
    {
        public Character[] Team { get; private set; }
        public PlayerData Player { get; private set; }//玩家数据
        public GameRound Round { get; private set; } //世界信息
        public RewardBoard RewardBoard { get; } = new RewardBoard();

        public void Init()
        {
            SendEvent(GameEvent.Game_Start);
            TestInit();
        }

        private void TestInit()
        {
            Player = new PlayerData(Game.Config.GetPresetPlayer(), Game.Config.CharacterAttributeMap);
            Team = Game.Config.GetCharacters().Select(r=>new Character(r)).ToArray();
            Round = new GameRound();
            Round.UpdatePurposes(Player);
            DebugInfo(Player);
            SendEvent(GameEvent.Episode_Start);
        }

        public void DebugInfo(PlayerData player)
        {
            var sb = new StringBuilder();
            sb.Append($"玩家：{player}\n武[{player.Power}]\n学[{player.Wisdom}]\n力[{player.Strength}]\n智[{player.Intelligent}]\n银[{player.Silver}]\n体[{player.Stamina}]");
            sb.Append(TagLog(player.Capable,"属性"));
            sb.Append(TagLog(player.Status,"状态"));
            sb.Append(TagLog(player.Skill,"技能"));
            sb.Append(TagLog(player.Inventory, "物品"));
            sb.Append(TagLog(player.Story, "故事"));
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

        public void SetCurrentPurpose(IPurpose purpose) => Round.SetPurpose(purpose);

        /// <summary>
        /// 尝试进行下个回合，返回不过的条件
        /// </summary>
        /// <returns></returns>
        public IPlotTerm[] TryProceedRound()
        {
            var notInTerms = Round.SelectedPurpose.GetOccasion(Player).GetExcludedTerms(Player);
            if (notInTerms.Any()) return notInTerms;
            RewardBoard.SetReward(Round.SelectedPurpose, Player);
            Round.NextRound(Player);
            return notInTerms;
        }
    }
}