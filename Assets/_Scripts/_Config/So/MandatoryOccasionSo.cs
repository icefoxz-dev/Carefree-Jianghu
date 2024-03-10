using System;
using System.Linq;
using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "MandatoryOccasionSo", menuName = "配置/场合/强制")]
    public class MandatoryOccasionSo : PurposeOccasionBase
    {
        [SerializeField] private ChallengeArgsBase challengeArgs;
        [SerializeField] private PlotTermField[] _terms;
        [SerializeField] private RewardJudge _reward;
        public override bool IsMandatory => true;

        public override IPlotTerm[] GetExcludedTerms(IRoleData role) =>
            _terms.GetExcludedTerms(role).ToArray();

        public override IValueTag[] GetRewards(IOccasionResult result) => _reward.GetRewards(result);
        public override IChallengeArgs ChallengeArgs => challengeArgs;

        [Serializable] private class RewardJudge
        {
            [SerializeField] private RewardTag[] _win;
            [SerializeField] private RewardTag[] _lose;

            public IValueTag[] GetRewards(IOccasionResult result) =>
                result.Result switch
                {
                    0 => _lose,
                    _ => _win
                };
        }

    }
}