using System.Collections.Generic;
using System.Linq;
using System.Text;
using _Data;
using Debug = UnityEngine.Debug;

namespace _Game._Models
{
    public class GameWorld : ModelBase
    {
        public Character[] Team { get; private set; }
        public RoleData Role { get; private set; }//玩家数据
        public GameRound Round { get; private set; } //世界信息
        public RewardBoard RewardBoard { get; } = new RewardBoard();

        public void Init()
        {
            SendEvent(GameEvent.Game_Start);
            TestInit();
        }

        private void TestInit()
        {
            Role = new RoleData(Game.Config.GetPresetPlayer());
            Team = Game.Config.GetCharacters().Select(r=>new Character(r)).ToArray();
            Round = new GameRound();
            Round.UpdatePurposes(Role);
            DebugInfo(Role);
            SendEvent(GameEvent.Episode_Start);
        }

        public void DebugInfo(RoleData role)
        {
            var sb = new StringBuilder();
            sb.Append($"<color=yellow>玩家：{role}\n武[{role.Power}]\n学[{role.Wisdom}]\n力[{role.Strength}]\n智[{role.Intelligent}]\n银[{role.Silver}]\n体[{role.Stamina}]");
            sb.Append(TagLog(role.Ability.Set,"属性"));
            sb.Append(TagLog(role.Status.Set,"状态"));
            sb.Append(TagLog(role.Skill.Set,"技能"));
            sb.Append(TagLog(role.Inventory.Set, "物品"));
            sb.Append(TagLog(role.Story.Set, "故事"));
            Debug.Log(sb);
            return;

            string TagLog(IEnumerable<ITagValue> tags, string tagName)
            {
                tags = tags.ToArray();
                var s = new StringBuilder();
                foreach (var val in tags)
                    s.Append($"\n{tagName}： {val.Tag.Name}: {val.Value}");
                return s.ToString();
            }
        }

        public void SetCurrentPurpose(IPurpose purpose) => Round.SelectPurpose(purpose);

        /// <summary>
        /// 尝试进行下个回合，返回不过的条件
        /// </summary>
        /// <returns></returns>
        public ITagTerm[] TryProceedRound()
        {
            var notInTerms = Round.SelectedPurpose.GetOccasion(Role).GetExcludedTerms(Role);
            if (notInTerms.Any()) return notInTerms;
            var purpose = Round.SelectedPurpose;
            var occasion = purpose.GetOccasion(Role);
            Round.InvokeChallenge(Role, result =>
            {
                var rewards = occasion.GetRewards(result);
                RewardBoard.SetReward(rewards, Role, occasion);
                Round.NextRound(Role);
                DebugInfo(Role);
            });
            return notInTerms;
        }
    }
}