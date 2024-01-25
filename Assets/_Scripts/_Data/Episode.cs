using System;

namespace _Data
{
    public interface IEpisode
    {
        int Id { get; }
        string Brief { get; }
        IOccasion[] Occasions { get; }
    }
    [Serializable]public record EpisodeData(int Id, string Brief, OccasionData[] OccasionData) : IEpisode
    {
        public int Id { get; } = Id;
        public string Brief { get; } = Brief;
        public IOccasion[] Occasions => OccasionData;
        public OccasionData[] OccasionData { get; } = OccasionData;
    }
}