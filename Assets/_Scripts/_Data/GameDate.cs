using System;
using UnityEngine;

namespace _Data
{
    public interface IGameDate
    {
        int Year { get; }
        int Month { get; }
        int Round { get; }
    }

    public class GameDate : IGameDate
    {
        public enum MonthPeriod
        {
            [InspectorName("月初")] Start,
            [InspectorName("上旬")] Early,
            [InspectorName("中旬")] Middle,
            [InspectorName("下旬")] Late,
            [InspectorName("月末")] End
        }

        public int Year { get; private set; }
        public int Month { get; private set; }
        public int Round { get; private set; }

        public int RoundToMonth { get; }

        public GameDate(int year, int month, int round, int roundToMonth = 5)
        {
            this.Year = year;
            this.Month = month;
            this.Round = round;
            RoundToMonth = roundToMonth;
        }

        public void NextRound()
        {
            if (Round < 1) RoundReset();
            Round++;
            if (Round <= RoundToMonth) return;
            if (Month < 1) MonthReset();
            RoundReset();
            Month++;
            if (Month <= 12) return;
            if(Year < 1) YearReset();
            Month = 1;
            Year++;
        }

        private void YearReset()=> Year = 1;
        private void MonthReset() => Month = 1;
        private void RoundReset() => Round = 1;
    }

    public static class GameDateExtension
    {
        public static GameDate.MonthPeriod GetRoundTitle(this IGameDate date)
        {
            return date.Round switch
            {
                1 => GameDate.MonthPeriod.Start,
                2 => GameDate.MonthPeriod.Early,
                3 => GameDate.MonthPeriod.Middle,
                4 => GameDate.MonthPeriod.Late,
                5 => GameDate.MonthPeriod.End,
                _ => throw new ArgumentOutOfRangeException($"Error on date.Round = {date.Round}")
            };
        }

        public static string GetRoundText(this IGameDate date)
        {
            return date.GetRoundTitle() switch
            {
                GameDate.MonthPeriod.Start => "月初",
                GameDate.MonthPeriod.Early => "上旬",
                GameDate.MonthPeriod.Middle => "中旬",
                GameDate.MonthPeriod.Late => "下旬",
                GameDate.MonthPeriod.End => "月末",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}