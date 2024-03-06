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
        public IGameTag GameTag => _gameTag;
        public string Name => _gameTag.Name;
        double IValueTag.Value => Value;
        [FormerlySerializedAs("GameTag")]public GameTagSoBase _gameTag;
        [FormerlySerializedAs("Compare"), SerializeField] public TagClauses _clause;
        public TagClauses Clause => _clause;
        public bool IsInTerm(IValueTag other) => this.IsInTerm(other, _clause);
        public bool IsInTerm(IRoleAttributes role) => role.GetAllTags().Any(IsInTerm);

        public ITagManager GetTagManager(IRoleAttributes attributes) => _gameTag.GetTagManager(attributes);
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