using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "ActivityConfigSo", menuName = "配置/Config/ActCfg")]
    public class ActivityConfigSo : ScriptableObject
    {
        [SerializeField] private FuncOccasionSo[] _occasions;
        public FuncOccasionSo[] GetOccasions() => _occasions;
    }
}