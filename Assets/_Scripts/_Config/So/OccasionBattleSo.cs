using _Data;
using System.Collections.Generic;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "OccasionBattleSo", menuName = "配置/场合/挑战/战斗")]
    public class BattleSo : ChallengeArgsBase,IChallengeBattleArgs
    {
        [SerializeField] private CharacterSo _opponent;
        [SerializeField] private BattleRewardJudge _judge;
        public override ChallengeTypes ChallengeType => ChallengeTypes.Battle;
        public override ITagValue[] GetRewards(IOccasionResult result) => _judge.GetRewards(result);
        public override void CheckTags()
        {
            _opponent.CheckTags();
            _judge.CheckTags(this);
        }

        public IRoleData GetOpponent(IEnumerable<IFormulaTag> capable) => _opponent.GetRoleData(capable);
    }
}