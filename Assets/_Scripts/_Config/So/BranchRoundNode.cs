using System;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Config.So
{
    /// <summary>
    /// 主要是用于判断回合数或者标签是否满足条件，并且分支。
    /// </summary>
    [CreateNodeMenu("BranchRound")]
    public class BranchRoundNode : EpRoundNode
    {
        [SerializeField] private RoundTriggers roundTrigger;
        [ConditionalField(nameof(roundTrigger), false, RoundTriggers.RoundCount), SerializeField] private RoundCountSet _roundSet;
        [ConditionalField(nameof(roundTrigger), false, RoundTriggers.GameTag), SerializeField,FormerlySerializedAs("_tagSet")] private TermSet _termSet;
    } 
    public enum RoundTriggers
        {
            [InspectorName("回合数")]RoundCount,
            [InspectorName("标签")]GameTag
        }
    [Serializable]public class TermSet
        {
            [SerializeField] private TagTermField[] _fields;

            //public bool IsTrigger(IRoleTerm role) =>
            //    _fields.Length == 0 || _fields.All(f => role.Tags.Any(t => f.IsInTerm(t, f._clause)));
        }

    [Serializable]
    public class RoundCountSet
    {
        private enum Options
        {
            [InspectorName("相等")] Equal,
            [InspectorName("大于")] Exceed,
            [InspectorName("小于")] Less
        }

        [SerializeField] private int _count;
        [SerializeField] private Options _option;

        public bool IsTrigger(int currentCount) => _option switch
        {
            Options.Equal => currentCount == _count,
            Options.Exceed => currentCount > _count,
            Options.Less => currentCount < _count,
            _ => false
        };
    }
}