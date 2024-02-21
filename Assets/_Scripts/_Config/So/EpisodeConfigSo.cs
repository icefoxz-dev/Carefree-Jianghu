using _Data;
using System;
using System.Linq;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "EpisodeConfigSo", menuName = "配置/Config/EpCfg")]
    public class EpisodeConfigSo : ScriptableObject
    {
        [SerializeField]private EpisodeGraphSo[] _episodes;
        public EpisodeData GetEpisode(int index) => _episodes[index].GetEpisode();
    }
}