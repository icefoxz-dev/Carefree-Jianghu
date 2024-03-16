using System.Linq;
using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "ActivityConfigSo", menuName = "配置/Config/ActCfg")]
    public class ActivityConfigSo : ScriptableObject, IStoryActivities
    {
        [SerializeField] private OccasionClusterSoBase[] _mandatory;
        public IPurpose[] GetMandatoryPurposes(IRoleData role, IGameRound round) =>
            _mandatory.SelectMany(c => c.GetPurposes(role, round)).ToArray();

        [SerializeField] private OccasionClusterSoBase[] _clusters;
        public IOccasionCluster[] GetClusters()
        {
            foreach (var soBase in _clusters) soBase.CheckTags();
            return _clusters;
        }
    }
}