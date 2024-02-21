using System.Collections.Generic;

namespace _Data
{
    /// <summary>
    /// 玩家属性
    /// </summary>
    public interface IPlayerProperty
    {
        ITagManager Trait { get; }
        ITagManager Capable { get; }
        ITagManager Skill { get; }
        ITagManager EpisodeTag { get; }
        ITagManager ChapterTag { get; }
    }

    /// <summary>
    /// 玩家角色数据，包含玩家的角色信息和故事进程。
    /// </summary>
    public interface IPlayerData
    {
        IPlayerProperty Prop { get; }
    }
    public interface ITagManager
    {
        IReadOnlyList<IFuncTag> Tags { get; }
        void AddTag(IFuncTag tag);
        void RemoveTag(IFuncTag tag);
        void AddTagValue(IFuncTag tag);
    }
}