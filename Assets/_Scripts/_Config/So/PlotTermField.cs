using System;
using _Data;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Config.So
{
    [Serializable]
    public class PlotTermField : IPlotTerm
    {
        public IGameTag GameTag => _gameTag;
        public string Name => _gameTag.Name;
        double IPlotTag.Value => Value;
        [FormerlySerializedAs("GameTag")]public GameTagSoBase _gameTag;
        [FormerlySerializedAs("Compare"), SerializeField] public PlotTagClause _clause;
        PlotTagClause IPlotTerm.Clause => _clause;
        bool IPlotTerm.IsInTerm(IPlotTag other) => this.IsInTerm(other, _clause);

        public ITagManager GetTagManager(IRoleProperty property) => _gameTag.GetTagManager(property);
        [ConditionalField(nameof(_clause), true, PlotTagClause.HasTag)] public double Value;
    }
}