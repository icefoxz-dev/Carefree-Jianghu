using System.Linq;
using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "TermOccasionSo", menuName = "配置/场合/条件")]
    public class TermOccasionSo : PurposeOccasionBase
    {
        [SerializeField] private OccasionBase _so;
        [SerializeField] private PlotTermField[] terms;

        public override IPlotTerm[] GetExcludedTerms(IRoleData role) =>
            terms.GetExcludedTerms(role).Concat(_so.GetExcludedTerms(role)).ToArray();

        public IOccasion GetOccasion(IRoleData role) => this;
        public override Occasion.Modes Mode => _so.Mode;
        public override string Description => _so.Description;

        public override IRolePlacing[] GetPlacingInfos() => _so.GetPlacingInfos();
        public override string GetLine(RolePlacing.Index role, int index) => _so.GetLine(role, index);
        public override void UpdateRewards(IRoleData role) => _so.UpdateRewards(role);
    }
}