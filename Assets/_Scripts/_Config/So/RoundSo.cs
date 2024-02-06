using System;
using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "RoundSo", menuName = "配置/活动/回合")]
    public class RoundSo : AutoUnderscoreNamingObject
    {
        [SerializeField] private OccasionField[] _activities;
        [Serializable] private class OccasionField 
        {
            
        }
    }
}