using System;
using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "OccasionSo", menuName = "配置/场合/一般")]
    public class FuncOccasionSo : OccasionSoBase
    {
        [SerializeField] private SceneContent _sceneContent;
        [SerializeField] private FuncTag[] results;

        public SceneContent SceneContent => _sceneContent;
        public override IFuncTag[] Results => results;

        [Serializable]
        private class FuncTag : IFuncTag
        {
            [SerializeField] private GameTagSoBase _gameTag;
            [SerializeField] private double _value = 1;
            public IGameTag GameTag => _gameTag;
            public double Value => _value;
        }
    }
}