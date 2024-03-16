using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "StoryMapSo", menuName = "配置/故事/地图")]
    public class StoryMapSo : AutoHashNamingObject,IStoryMap
    {
        [SerializeField] private ActivityConfigSo _activitySo;
        [SerializeField, TextArea] private string[] _intro;
        public ActivityConfigSo ActivitySo => _activitySo;
        public IStoryActivities Activities => _activitySo;
        public string[] Intro => _intro;
    }
}