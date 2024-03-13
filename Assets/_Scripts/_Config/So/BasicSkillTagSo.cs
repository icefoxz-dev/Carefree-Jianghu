using System;
using System.Linq;
using _Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "BasicSkillTagSo", menuName = "配置/标签/技能/基本功")]
    public class BasicSkillTagSo : SkillTagSo, IBasicSkill
    {
        public override SkillType SkillType => SkillType.Basic;
        public override double GetPower(int level, IRoleData role) => level * 0.5;
    }

    public abstract class SkillTagSo: RoleTagSoBase,ISkillTag
    {
        public override TagType TagType => TagType.Skill;
        public abstract SkillType SkillType { get; }
        public abstract double GetPower(int level, IRoleData role);
    }

    [Serializable]
    public class SkillLevelMap
    {
        [SerializeField] private bool _useCurve;
        [ShowIf(nameof(_useCurve)), SerializeField] private CurveMap _curve;
        [HideIf(nameof(_useCurve)), SerializeField] private CustomMap _custom;


        public double GetValue(int level)
        {
            if (_useCurve) return _curve.Evaluate(level);
            return _custom.GetValue(level);
        }
        [Button]private void SetFromCurve() => _custom.SetFromCurve(_curve);

        [Serializable]
        private class CurveMap
        {
            [SerializeField] private AnimationCurve _curve;
            public double _min;
            public double _max;
            public int _levels;

            public double Evaluate(int level) => _curve.Evaluate(Mathf.Lerp(0, 1, level / (float)_levels)) * (_max - _min) + _min;
        }

        [Serializable]private class CustomMap
        {
            [SerializeField] private LevelMap[] _fields;

            public double GetValue(int level)
            {
                var lvl = _fields.First(m => m.Level == level);
                return lvl.Power;
            }

            public void SetFromCurve(CurveMap map)
            {
                _fields = new LevelMap[map._levels];
                for (var i = 0; i < map._levels; i++)
                {
                    var level = i+1;
                    _fields[i] = new LevelMap
                    {
                        Level = level,
                        Power = map.Evaluate(level)
                    };
                }
            }

            [Serializable]
            private class LevelMap
            {
                [HorizontalGroup] public int Level;
                [HorizontalGroup] public double Power;
            }
        }
    }
}