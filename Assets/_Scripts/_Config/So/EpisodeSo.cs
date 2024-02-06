using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "EpisodeSo", menuName = "配置/新/剧集")]
    public class EpisodeSo : AutoUnderscoreNamingObject
    {
        [TextArea]public string Description;
        public int MaxRound = -1;
    }
}