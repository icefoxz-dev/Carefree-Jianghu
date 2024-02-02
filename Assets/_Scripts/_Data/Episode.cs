using System;
using System.Collections.Generic;

namespace _Data
{
    public interface IEpisode
    {
        int Id { get; }
        string Brief { get; }
        Dictionary<int, List<IEpisodeNode>> Map { get; }
    }
    [Serializable]public record EpisodeData(
        int Id,
        string Brief,
        IOccasion[] Occasions,
        Dictionary<int, List<IEpisodeNode>> Map) : IEpisode
    {
        public int Id { get; } = Id;
        public string Brief { get; } = Brief;
        public IOccasion[] Occasions { get; } = Occasions;
        public Dictionary<int, List<IEpisodeNode>> Map { get; } = Map;
    }
}