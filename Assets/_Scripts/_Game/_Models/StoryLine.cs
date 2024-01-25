using System.Collections;

namespace _Game._Models
{
    /// <summary>
    /// todo: 这个未定义清楚是什么
    /// 故事线, 这个类表示贯穿整个游戏的主要故事线。它包含多个故事章节<see cref="Chapter"/>和分支，可以根据玩家的选择和角色的发展动态改变。
    /// </summary>
    public class StoryLine 
    {
        public string Name { get; }
    }
    /// <summary>
    /// 故事章节, 这个类代表故事线<see cref="StoryLine"/>中的一个分段。每个章节是主故事线的一个节点，包含多个故事剧情和可能的分支。<see cref="IPlot"/>
    /// </summary>
    public abstract class ChapterBase : IPlot
    {
        protected EpisodeBase[] Episodes { get; }
        protected ChapterBase(EpisodeBase[] episodes)
        {
            Episodes = episodes;
        }
        public abstract IPlot Next();
    }

    /// <summary>
    /// 故事的情节结构和发展流程, 代表故事的分支。每个分支提供不同的故事走向，根据玩家的选择和角色的状态变化而动态展开。
    /// </summary>
    public interface IPlot
    {
        IPlot Next();
    }
}