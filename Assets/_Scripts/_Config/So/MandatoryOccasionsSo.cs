using System;
using System.Collections.Generic;
using System.Linq;
using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "MandatoryOccasionsSo", menuName = "配置/场合/时间强制")]
    public  class MandatoryOccasionsSo : OccasionClusterSoBase
    {
        [SerializeField] private TagTermField[] _terms;
        [SerializeField] private MandatoryOccasionSo _occasion;

        protected override IEnumerable<IPurpose> GetOccasionPurpose(IRoleData role, IGameRound round) =>
            _terms.All(t => t.IsInTerm(role.Attributes))
                ? _occasion.IsInTerm(role) ? new IPurpose[] { _occasion } : Array.Empty<IPurpose>()
                : Array.Empty<IPurpose>();

        public override void CheckTags() => _occasion.CheckTags();
    }
}