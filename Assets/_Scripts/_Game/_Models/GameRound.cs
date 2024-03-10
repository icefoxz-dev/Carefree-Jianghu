using System;
using _Data;
using System.Linq;
using UnityEngine.Events;

namespace _Game._Models
{
    /// <summary>
    /// 游戏回合，处理回合，游戏时间等逻辑
    /// </summary>
    public class GameRound : ModelBase , IGameRound
    {
        public int Year => GameDate.Year;
        public int Month => GameDate.Month;
        public int Round => GameDate.Round;

        private GameDate GameDate { get; set; } = new GameDate(1,1,1);
        public IPurpose[] Purposes { get; private set; }
        //public IChallenge Challenge { get; private set; }
        public IPurpose SelectedPurpose { get; private set; }
        public Challenge.SimpleBattle Battle { get; private set; }
        public ChallengeTypes ChallengeType { get; private set; }

        public void NextRound(IRoleData role)
        {
            GameDate.NextRound();
            SelectedPurpose = null;
            UpdatePurposes(role);
        }

        /// <summary>
        /// 更新可执行的意图
        /// </summary>
        /// <param name="role"></param>
        public void UpdatePurposes(IRoleData role)
        {
            var clusters = Game.Config.ActivityCfg.GetClusters();
            var purposes = clusters.SelectMany(c => c.GetPurposes(role, this)).ToArray();
            var mandotary = purposes.Where(p => p.IsMandatory).ToArray();
            Purposes = mandotary.Length > 0
                ? mandotary
                : purposes;
            SendEvent(GameEvent.Round_Update);
        }
        /// <summary>
        /// 选择意图
        /// </summary>
        /// <param name="purpose"></param>
        public void SelectPurpose(IPurpose purpose)
        {
            SelectedPurpose = purpose;
            SendEvent(GameEvent.Round_Update);
        }

        public void InvokeChallenge(IRoleData role, UnityAction<IOccasionResult> callback)
        {
            var occasion = SelectedPurpose.GetOccasion(role);
            ChallengeType = occasion.ChallengeArgs.ChallengeType;
            switch (occasion.ChallengeArgs.ChallengeType)
            {
                case ChallengeTypes.Battle:
                    var arg = (IChallengeBattleArgs)occasion.ChallengeArgs;
                    var opponent = arg.GetOpponent(Game.Config.CharacterTagsMap.GetCapableTags);
                    Battle = new Challenge.SimpleBattle(role, opponent, r => OnStartBattle(r, callback));
                    break;
                case ChallengeTypes.MiniGame:
                    break;
                case ChallengeTypes.None:
                    callback(Challenge.DefaultResult);
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            SendEvent(GameEvent.Challenge_Start);
        }

        private void OnStartBattle(IOccasionResult result, UnityAction<IOccasionResult> callback)
        {
            callback(result);
            Battle = null;
            SendEvent(GameEvent.Battle_End);
        }
    }
}