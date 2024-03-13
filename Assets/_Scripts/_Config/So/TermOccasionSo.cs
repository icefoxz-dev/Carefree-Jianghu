using System.Linq;
using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "TermOccasionSo", menuName = "配置/场合/条件")]
    public class TermOccasionSo : PurposeOccasionBase,IChallengeArgs
    {
        [SerializeField] private OccasionBase _so;
        [SerializeField] private TagTermField[] terms;

        public override ITagTerm[] GetExcludedTerms(IRoleData role) =>
            terms.GetExcludedTerms(role).Concat(_so.GetExcludedTerms(role)).ToArray();

        public override IValueTag[] GetRewards(IOccasionResult result) => _so.GetRewards(result);

        public override IChallengeArgs ChallengeArgs => this;
        public ChallengeTypes ChallengeType => ChallengeTypes.None;
        public override Occasion.Modes Mode => _so.Mode;
        public override string Description => _so.Description;
        public override bool IsMandatory => false;

        public override IRolePlacing[] GetPlacingInfos() => _so.GetPlacingInfos();
        public override string GetLine(RolePlacing.Index role, int index) => _so.GetLine(role, index);

    }
}