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
            public bool IsPlayerWin { get; private set; }
            public IOccasionResult Result { get; private set; }
            private UnityAction<IOccasionResult> _callback;

            public SimpleBattle(IRoleData player, IRoleData opponent, UnityAction<IOccasionResult> callback)
            {
                _callback = callback;
                Player = player;
                Opponent = opponent;
            }

            public void Start()
            {
                var differ = Player.Power() - Opponent.Power();
                IsPlayerWin = differ >= 0;
                Result = new OccasionResult(IsPlayerWin ? 1 : 0);
            }

            public void Finalize()
            {
                _callback(Result);
                _callback = null;
            }
        }

        private record OccasionResult(int Result) : IOccasionResult
        {
            public int Result { get; } = Result;
        }
    }
}