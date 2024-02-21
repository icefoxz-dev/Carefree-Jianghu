using System.Linq;
using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "RoleConfigSo", menuName = "配置/Config/RoleCfg")]
    public class RoleConfigSo : ScriptableObject
    {
        [SerializeField]private CharacterSo[] _roles;
        public ICharacter GetCharacter(int index) => _roles[index];

        public ICharacter[] GetCharacters()=>_roles;
    }
}