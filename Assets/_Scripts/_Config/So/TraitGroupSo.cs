using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "TraitGroupSo", menuName = "配置/角色/标签集")]
    public class TraitGroupSo: AutoAtNamingObject
    {
        [SerializeField]private TraitSo[] traits;
    }
}