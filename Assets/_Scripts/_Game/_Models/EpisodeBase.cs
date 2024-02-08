using System.Collections.Generic;
using System.Linq;
using _Data;

namespace _Game._Models
{
    /// <summary>
    /// 剧集, 这个类代表故事的一个独立片段。它是玩家在特定场景中交互的基本单位，根据故事的配置和玩家的行为确定下一步的故事走向。
    /// </summary>
    public abstract class EpisodeBase : ModelBase, IEpisode
    {

        public int Id { get; }
        public string Brief { get; }
        public Dictionary<int,EpisodeFrame> FrameMap { get; }

        protected EpisodeBase(IEpisode ep)
        {
            Id = ep.Id;
            Brief = ep.Brief;
            FrameMap = new Dictionary<int, EpisodeFrame>();
        }

        public void SetOccasion(int occasionIndex, RolePlacing.Index place, Character character)
        {
            FrameMap[occasionIndex].PlaceCharacter(place,character);
            SendEvent(GameEvent.Episode_Occasion_Update, occasionIndex);
        }
    }

    public class TestEpisode : EpisodeBase
    {
        public TestEpisode(IEpisode ep): base(ep)
        {
        }
    }
}