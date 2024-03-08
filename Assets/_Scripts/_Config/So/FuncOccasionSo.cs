using System;
using System.Linq;
using _Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "OccasionSo", menuName = "配置/场合/一般")]
    public class FuncOccasionSo : PurposeOccasionBase
    {
        [SerializeField] private SceneContent _sceneContent;
        [SerializeField,FormerlySerializedAs("results")] private ValueTag[] rewards;
        [SerializeField] private PlotTermField[] terms;

        public SceneContent SceneContent => _sceneContent;

        public override void UpdateRewards(IRoleData role)
        {
            foreach (var tag in rewards.Select(t => t._gameTag))
                if (!tag)
                    Debug.LogError("game tag not set!", this);
            foreach (var tag in rewards) tag.UpdateRole(role);
        }

        public override IPlotTerm[] GetExcludedTerms(IRoleData role)
        {
            foreach (var tag in terms.Select(t=>t._gameTag))
                if (!tag)
                    Debug.LogError("game tag not set!", this);
            return terms.GetExcludedTerms(role);
        }

        [Serializable]
        private class ValueTag : IValueTag
        {
            [SerializeField] public GameTagSoBase _gameTag;
            [SerializeField] private double _value = 1;

            public double Value=> _value;
            public IGameTag GameTag => _gameTag;
            public string Name => _gameTag.Name;
            public ITagManager GetTagManager(IRoleAttributes attributes) => _gameTag.GetTagManager(attributes);
        }
    }
}