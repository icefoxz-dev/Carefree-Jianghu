using System.Linq;
using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "ActivityConfigSo", menuName = "配置/Config/ActCfg")]
    public class ActivityConfigSo : ScriptableObject
    {
        [SerializeField] private OccasionClusterSoBase[] _clusters;
        public IOccasionCluster[] GetClusters() => _clusters;
    }
}