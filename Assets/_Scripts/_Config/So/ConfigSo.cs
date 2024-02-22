using System;
using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "ConfigSo", menuName = "配置/Config/ConfigSo")]
    public class ConfigSo :ScriptableObject
    {
        public EpisodeData GetEpisode(int index) => _epCfg.GetEpisode(index);
        [SerializeField] private EpisodeConfigSo _epCfg;
        public ICharacter GetCharacter(int index) => _roleCfg.GetCharacter(index);
        public ICharacter[] GetCharacters() => _roleCfg.GetCharacters();
        [SerializeField] private RoleConfigSo _roleCfg;
        [SerializeField] private CharacterSo _player;
        public IRoleData GetPresetPlayer() => _player.GetRoleData();
        [SerializeField] private CharacterAttributeMapSo _characterAttributeMap;
        public ICharacterAttributeMap CharacterAttributeMap => _characterAttributeMap;
    }
}