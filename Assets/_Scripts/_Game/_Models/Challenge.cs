using _Data;
using UnityEngine.Events;

namespace _Game._Models
{
    /// <summary>
    /// 挑战类，主要实现与玩家的交互
    /// </summary>
    public static class Challenge
    {
        public static readonly IOccasionResult DefaultResult = new OccasionResult(0);

        public class SimpleBattle
        {
            public IRoleData Opponent { get; }
            public IRoleData Player { get; }
            private UnityAction<IOccasionResult> _callback;

            public SimpleBattle(IRoleData player,IRoleData opponent, UnityAction<IOccasionResult> callback)
            {
                Player = player;
                Opponent = opponent;
                _callback = callback;
            }

            public void Start()
            {
                var differ = Player.Power() - Opponent.Power();
                var isWin = differ >= 0;
                _callback(new OccasionResult(isWin ? 1 : 0));
            }
        }

        private record OccasionResult(int Result) : IOccasionResult
        {
            public int Result { get; } = Result;
        }
    }
}