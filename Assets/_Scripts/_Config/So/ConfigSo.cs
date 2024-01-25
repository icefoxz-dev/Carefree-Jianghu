using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "ConfigSo", menuName = "配置/Config/ConfigSo")]
    public class ConfigSo :ScriptableObject
    {
        public EpisodeData GetEpisode(int index) => _epCfg.GetEpisode(index);
        [SerializeField] private EpisodeConfigSo _epCfg;
        public IRolePlay GetRole(int index) => _roleCfg.GetRole(index);
        public IRolePlay[] GetRoles() => _roleCfg.GetRoles();
        [SerializeField] private RoleConfigSo _roleCfg;
    }
}