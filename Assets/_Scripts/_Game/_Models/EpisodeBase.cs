using System.Collections.Generic;
using System.Linq;
using _Data;

namespace _Game._Models
{
    /// <summary>
    /// 剧集, 这个类代表故事的一个独立片段。它是玩家在特定场景中交互的基本单位，根据故事的配置和玩家的行为确定下一步的故事走向。<see cref="IPlot"/>
    /// </summary>
    public abstract class EpisodeBase : ModelBase, IPlot, IEpisode
    {
        public abstract int Id { get; }
        public string Brief { get; }
        IOccasion[] IEpisode.Occasions => Occasions;

        protected abstract EpisodeFrame[] Occasions { get; }
        public abstract IPlot Next();

        public EpisodeFrame GetOccasion(int index) => Occasions[index];

        public void SetOccasion(int occasionIndex, int placeIndex, RolePlay rolePlay)
        {
            Occasions[occasionIndex].SetRole(rolePlay, placeIndex);
            SendEvent(GameEvent.Episode_Occasion_Update, occasionIndex);
        }
    }

    public class TestEpisode : EpisodeBase
    {
        public TestEpisode(IEpisode ep)
        {
            Id = ep.Id;
            Occasions = ep.Occasions.Select(o => new EpisodeFrame(o)).ToArray();
        }

        public override int Id { get; }

        protected override EpisodeFrame[] Occasions { get; }
        public override IPlot Next() => null;
    }
}