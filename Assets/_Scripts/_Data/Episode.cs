using System;

namespace _Data
{
    public interface IRoundNode
    {

    }
    public interface IEpisode
    {
        int Id { get; }
        string Brief { get; }
    }
    [Serializable]public record EpisodeData(
        int Id,
        string Brief,
        IOccasion[] Occasions) : IEpisode
    {
        public int Id { get; } = Id;
        public string Brief { get; } = Brief;
        public IOccasion[] Occasions { get; } = Occasions;
    }
}