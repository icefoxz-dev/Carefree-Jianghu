using System;
using System.Collections.Generic;
using System.Linq;
using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "OccasionsClusterSo", menuName = "配置/场合/组")]
    public class OccasionsClusterSo : OccasionClusterSoBase
    {
        [SerializeField] private PlotTermField[] _terms;
        [SerializeField] private PurposeOccasionBase[] _occasions;

        public override IEnumerable<IPurpose> GetPurposes(IRoleData role)=>_occasions.Where(f=>f.IsInTerm(role)).ToArray();
    }

    public abstract class OccasionClusterSoBase : AutoAtNamingObject, IOccasionCluster
    {
        public abstract IEnumerable<IPurpose> GetPurposes(IRoleData role);
    }
}