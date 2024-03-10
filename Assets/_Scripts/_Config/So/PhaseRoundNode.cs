using System;
using System.Linq;
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
                [SerializeField] private TagClauses clauses;
                [SerializeField] private RoleTagSoBase _tag;
                [SerializeField] private double _value;

                public IRoleTag Tag => _tag;
                public string Name => _tag.Name;
                public double Value => _value;
                public ITagManager GetTagManager(IRoleAttributes attributes) => _tag.GetTagManager(attributes);
                public TagClauses Clause => clauses;
                public bool IsInTerm(IValueTag other) => this.IsInTerm(other, Clause);
                public bool IsInTerm(IRoleAttributes role) => role.GetAllTags().Any(IsInTerm);
            }
        }
    }
}