using System.Linq;
using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "RoleConfigSo", menuName = "配置/Config/RoleCfg")]
    public class RoleConfigSo : ScriptableObject
    {
        [SerializeField]private RoleSo[] _roles;
        public IRolePlay GetRole(int index) => _roles[index].GetRole();

        public IRolePlay[] GetRoles()=>_roles.Select(role => role.GetRole()).ToArray();
    }
}