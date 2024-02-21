using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "ActivityConfigSo", menuName = "配置/Config/ActCfg")]
    public class ActivityConfigSo : ScriptableObject
    {
        [SerializeField] private OccasionSo[] _occasions;
        public OccasionSo[] GetOccasions() => _occasions;
    }
}