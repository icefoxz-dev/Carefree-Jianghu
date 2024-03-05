using System;
using System.Linq;
using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "OccasionSo", menuName = "配置/场合/一般")]
    public class FuncOccasionSo : OccasionSoBase
    {
        [SerializeField] private SceneContent _sceneContent;
        [SerializeField] private ValueTag[] results;
        [SerializeField] private PlotTermField[] terms;

        public SceneContent SceneContent => _sceneContent;

        public override void UpdateRole(IRoleData role)
        {
            foreach (var tag in results.Select(t => t._gameTag))
                if (!tag)
                    Debug.LogError("game tag not set!", this);
            foreach (var tag in results) tag.UpdateRole(role);
        }

        public override IPlotTerm[] GetExcludedTerms(IRoleData role)
        {
            foreach (var tag in terms.Select(t=>t._gameTag))
                if (!tag)
                    Debug.LogError("game tag not set!", this);
            return terms.Where(t => !t.IsInTerm(role.Attributes)).ToArray();
        }

        [Serializable]
        private class ValueTag : IValueTag
        {
            [SerializeField] public GameTagSoBase _gameTag;
            [SerializeField] private double _value = 1;

            public double Value=> _value;
            public string Name => _gameTag.Name;
            public ITagManager GetTagManager(IRoleAttributes attributes) => _gameTag.GetTagManager(attributes);
        }
    }
}