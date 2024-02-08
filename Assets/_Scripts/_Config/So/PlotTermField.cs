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
        string IGameTag.Name => GameTag.Name;

        double IPlotTag.Value => Value;
        public GameTag GameTag;
        [FormerlySerializedAs("Compare"), SerializeField] public PlotTagClause _clause;
        PlotTagClause IPlotTerm.Clause => _clause;
        bool IPlotTerm.IsInTerm(IPlotTag other) => this.IsInTerm(other, _clause);

        public ITagManager GetTagManager(IPlayerProperty property) => GameTag.GetTagManager(property);
        [ConditionalField(nameof(_clause), true, PlotTagClause.HasTag)] public double Value;
    }
}