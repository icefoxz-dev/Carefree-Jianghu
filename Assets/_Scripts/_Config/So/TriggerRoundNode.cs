using System;
using System.Linq;
using _Data;
using MyBox;
using UnityEngine;

namespace _Config.So
{
    [CreateNodeMenu("TriggerRound")]
    public class TriggerRoundNode : EpRoundNode
    {
        public enum TriggerTypes
        {
            [InspectorName("回合数")]RoundCount,
            [InspectorName("标签")]GameTag
        }
        [SerializeField] private TriggerTypes _triggerType;

        [ConditionalField(nameof(_triggerType), false, TriggerTypes.RoundCount), SerializeField] private RoundCountSet _roundSet;
        [ConditionalField(nameof(_triggerType), false, TriggerTypes.GameTag), SerializeField] private TagSet _tagSet;

        [Serializable]private class TagSet
        {
            [SerializeField] private PlotTermField[] _fields;

            public bool IsTrigger(IRoleTerm role) =>
                _fields.Length == 0 || _fields.All(f => role.Tags.Any(t => f.IsInTerm(t, f._clause)));
        }
        [Serializable]private class RoundCountSet
        {
            private enum Options
            {
                [InspectorName("相等")]Equal,
                [InspectorName("大于")]Exceed,
                [InspectorName("小于")]Less
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
}