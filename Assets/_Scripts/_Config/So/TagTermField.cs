using System;
using System.Linq;
using _Data;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Config.So
{
    [Serializable]
    public class TagTermField : ITagTerm
    {
        public IGameTag Tag => RoleTag;
        public string Name => RoleTag.Name;

        public TagType TagType => RoleTag.TagType;
        double ITagValue.Value => Value;

        [FormerlySerializedAs("GameTag")]public RoleTagSoBase RoleTag;
        [FormerlySerializedAs("Compare"), SerializeField] public TagClauses _clause;
        public TagClauses Clause => _clause;
        public bool IsInTerm(ITagValue other) => this.IsInTerm(other, _clause);
        public bool IsInTerm(IRoleAttributes role) => role.GetAllTags().Any(IsInTerm);
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