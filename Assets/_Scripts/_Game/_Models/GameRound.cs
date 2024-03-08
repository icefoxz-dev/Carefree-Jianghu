using _Data;
using System.Linq;

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

        public IPurpose SelectedPurpose { get; private set; }

        public void NextRound(IRoleData role)
        {
            GameDate.NextRound();
            SelectedPurpose = null;
            UpdatePurposes(role);
        }

        public void UpdatePurposes(IRoleData role)
        {
            var clusters = Game.Config.ActivityCfg.GetClusters();
            Purposes = clusters.SelectMany(c => c.GetPurposes(role, this)).ToArray();
            SendEvent(GameEvent.Round_Update);
        }

        public void SetPurpose(IPurpose purpose)
        {
            SelectedPurpose = purpose;
            SendEvent(GameEvent.Round_Update);
        }
    }
}