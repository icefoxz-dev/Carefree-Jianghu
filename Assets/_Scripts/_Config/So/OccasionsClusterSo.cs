using System;
using System.Collections.Generic;
using System.Linq;
using _Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "OccasionsClusterSo", menuName = "配置/场合/组")]
    public class OccasionsClusterSo : OccasionClusterSoBase
    {
        [SerializeField] private TagTermField[] _terms;
        [SerializeField] private PurposeOccasionBase[] _occasions;

        protected override IEnumerable<IPurpose> GetOccasionPurpose(IRoleData role, IGameRound round) =>
            _terms.All(t => t.IsInTerm(role.Attributes))
                ? _occasions.Where(f => f.IsInTerm(role)).ToArray()
                : Array.Empty<IPurpose>();

    }

    public abstract class OccasionClusterSoBase : AutoAtNamingObject, IOccasionCluster
    {
        private enum TimeTerms
        {
            [InspectorName("当天")] Exact,
            [InspectorName("开始")] Begin,
            [InspectorName("结束")] End,
        }
        [SerializeField] private TimeTermField[] _timeTerms;

        public IEnumerable<IPurpose> GetPurposes(IRoleData role, IGameRound gameRound) =>
            _timeTerms.All(t => t.IsInTerm(gameRound)) ? GetOccasionPurpose(role, gameRound) : Array.Empty<IPurpose>();

        protected abstract IEnumerable<IPurpose> GetOccasionPurpose(IRoleData role, IGameRound round);

        [Serializable] private class TimeTermField
        {
            [SerializeField] private TimeTerms _term;
            [SerializeField, HorizontalGroup("1")] public int _year = 1;
            [SerializeField, HorizontalGroup("1")] public int _month = 1;
            [SerializeField, HorizontalGroup("1")] public GameDate.MonthPeriod _round;
            public bool IsInTerm(IGameDate date)
            {
                switch (_term)
                {
                    case TimeTerms.Exact:
                        var d = InstanceGameDate(_year,_month,(int)_round);
                        return date.Year == d.Year && date.Month == d.Month && date.Round == d.Round;
                    case TimeTerms.Begin:
                        return IsBeginAt(date, InstanceGameDate(_year, _month, (int)_round));
                    case TimeTerms.End:
                        return IsEndAt(date, InstanceGameDate(_year, _month, (int)_round));
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            private GameDateField InstanceGameDate(int year,int month,int round)
            {
                if (year < 1 || month < 1) throw new ArgumentOutOfRangeException($"yr({year}), m({month}), r({round})!");
                return new GameDateField(year, month, round);
            }

            private bool IsBeginAt(IGameDate date, GameDateField set, bool included = true)
            {
                if (date.Year > set.Year) return true;
                if (date.Year < set.Year) return false;
                if (date.Month > set.Month) return true;
                if (date.Month < set.Month) return false;
                return included ? date.Round >= set.Round : date.Round > set.Round;
            }

            private bool IsEndAt(IGameDate date, GameDateField set, bool included = true)
            {
                if (date.Year < set.Year) return true;
                if (date.Year > set.Year) return false;
                if (date.Month < set.Month) return true;
                if (date.Month > set.Month) return false;
                return included ? date.Round <= set.Round : date.Round < set.Round;
            }

            private struct GameDateField : IGameDate
            {
                public int Year { get; }
                public int Month { get; }
                public int Round { get; }

                public GameDateField(int year, int month, int round)
                {
                    Year = year;
                    Month = month;
                    Round = round;
                }
            }
        }
    }
}