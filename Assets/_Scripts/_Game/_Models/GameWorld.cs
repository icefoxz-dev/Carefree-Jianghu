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
        public RoleData MainRole { get; private set; }//玩家数据
        public GameRound Round { get; private set; } //世界信息
        public RewardBoard RewardBoard { get; } = new RewardBoard();

        public void Init()
        {
            SendEvent(GameEvent.Game_Start);
            TestRound();
            return;

            void TestRound()
            {
                MainRole = new RoleData(Game.Config.GetPresetPlayer());
                Team = Game.Config.GetCharacters().Select(r => new Character(r)).ToArray();
                SetNewGameRound(new GameRound(Game.Config.StoryMap, new GameDate(1, 1, 0)));
                DebugInfo(MainRole);
                SendEvent(GameEvent.Episode_Start);
            }
        }

        private void SetNewGameRound(GameRound gameRound)
        {
            Round = gameRound;
            SendEvent(GameEvent.Round_New);
            BeginRound();
        }

        public void DebugInfo(RoleData role)
        {
            var sb = new StringBuilder();
            sb.Append($"<color=yellow>玩家：{role} 武[{role.Power}] 学[{role.Wisdom}] 力[{role.Strength}] 智[{role.Intelligent}] 银[{role.Silver}] 体[{role.Stamina}]</color>");
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

        /// <summary>
        /// 选中当前的意图
        /// </summary>
        /// <param name="purpose"></param>
        public void SetCurrentPurpose(IPurpose purpose) => Round.SelectPurpose(MainRole, purpose);

        /// <summary>
        /// 确定意图，执行活动生成挑战等待结果。
        /// </summary>
        /// <returns></returns>
        public void InstanceChallenge()
        {
            Round.InstanceChallenge(MainRole, SetResultProcessNextRound);
            return;

            void SetResultProcessNextRound(IOccasionResult result)
            {
                var occasion = Round.Occasion;
                var rewards = occasion.ChallengeArgs.GetRewards(result);
                RewardBoard.SetReward(rewards, MainRole);
                NextRound();
            }
        }

        // 执行下个回合
        private void NextRound()
        {
            Round.NextRound();
            BeginRound();
            DebugInfo(MainRole);
        }

        // 开始回合，如果是强制活动，直接选择中
        private void BeginRound()
        {
            Round.UpdatePurposes(MainRole);
            if (!Round.IsMandatory) return;
            SetCurrentPurpose(Round.Purposes.First());
        }

        //目前没有特别执行，但为了保持一致性，不让外部调用Round的ChallengeStart和ChallengeFinalize
        public void StartChallenge() => Round.ChallengeStart();
        public void FinalizeChallenge() => Round.ChallengeFinalize();
    }
}