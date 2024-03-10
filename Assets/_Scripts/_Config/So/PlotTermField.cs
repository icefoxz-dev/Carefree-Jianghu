using System;
using System.Linq;
using _Data;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Config.So
{
    [Serializable]
    public class PlotTermField : IPlotTerm
    {
        public IRoleTag Tag => RoleTag;
        public string Name => RoleTag.Name;
        double IValueTag.Value => Value;
        [FormerlySerializedAs("GameTag")]public RoleTagSoBase RoleTag;
        [FormerlySerializedAs("Compare"), SerializeField] public TagClauses _clause;
        public TagClauses Clause => _clause;
        public bool IsInTerm(IValueTag other) => this.IsInTerm(other, _clause);
        public bool IsInTerm(IRoleAttributes role) => role.GetAllTags().Any(IsInTerm);

        public ITagManager GetTagManager(IRoleAttributes attributes) => RoleTag.GetTagManager(attributes);
        public double Value;

        public override string ToString() => $"{Name}: {GetClauseText(_clause)}{Value}";

        private string GetClauseText(TagClauses clauses)
        {
            return clauses switch
            {
                TagClauses.Equal => "==",
                TagClauses.Exceed => ">",
                TagClauses.Less => "<",
                _ => throw new ArgumentOutOfRangeException(nameof(clauses), clauses, null)
            };
        }
    }
}