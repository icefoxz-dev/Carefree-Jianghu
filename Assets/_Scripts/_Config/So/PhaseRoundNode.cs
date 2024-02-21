using System;
using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateNodeMenu("PhaseRound"), NodeTint(0.30f, 0.36f, 0.4f)]
    public class PhaseRoundNode : EpRoundNode
    {
        [Serializable] private class ConditionField
        {

            [Serializable]
            private class TagCondition : IPlotTerm
            {
                [SerializeField] private PlotTagClause _clause;
                [SerializeField] private GameTag _tag;
                [SerializeField] private double _value;

                public IGameTag GameTag => _tag;
                public string Name => _tag.Name;
                public double Value => _value;
                public ITagManager GetTagManager(IPlayerProperty property) => _tag.GetTagManager(property);
                public PlotTagClause Clause => _clause;
                public bool IsInTerm(IPlotTag other) => this.IsInTerm(other, Clause);
            }
        }
    }
}