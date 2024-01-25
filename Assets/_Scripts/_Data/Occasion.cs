using System;
using UnityEngine;
using UnityEngine.Events;

namespace _Data
{
    /// <summary>
    /// 场合静态类，定义各种规范和工具
    /// </summary>
    public static class Occasion
    {
        public enum PlaceMode
        {
            Solo,
            Versus,
        }
        public enum Phase
        {
            Root,
            Transition,
            End
        }
    }
    
    public interface IEpisodeNode
    {
        Occasion.Phase Phase { get; }
        IEpisodeNode[] GetNextNodes();
        IOccasion Occasion { get; }
        /// <summary>
        /// 获取Node在场合中的顺序, 如果不在<see cref="IRoleTerm"/>条件内，返回-1
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        int GetEpNodeOrder(IRoleTerm role);
    }
    /// <summary>
    /// 故事场景, 表示玩家可以互动的具体场景。这是玩家在故事剧情中做出选择和行动的地方，直接影响故事的发展。
    /// </summary>
    public interface IOccasion
    {
        Occasion.PlaceMode PlaceMode { get; }
        IEpisodeNode EpNode { get; }
        string Name { get; }
        string Description { get; }
        /// <summary>
        /// 角色
        /// </summary>
        IRolePlay[] Roles { get; }
        ISceneContent SceneContent { get; }
    }

    public interface ISceneContent
    {
        Transform Bg { get; }
        void Play();
        UnityEvent<Role.Index, int> OnRoleLineEvent { get; }
        UnityEvent OnEndEvent { get; }
    }

    [Serializable]
    public record OccasionData(Occasion.PlaceMode PlaceMode, string Name, string Description, ISceneContent SceneContent, IEpisodeNode EpNode) : IOccasion
    {
        public Occasion.PlaceMode PlaceMode { get; } = PlaceMode;
        public IEpisodeNode EpNode { get; } = EpNode;
        public string Name { get; } = Name;
        public string Description { get; } = Description;
        public IRolePlay[] Roles { get; } = PlaceMode == Occasion.PlaceMode.Versus ? new IRolePlay[2] : new IRolePlay[1];
        public ISceneContent SceneContent { get; } = SceneContent;
    }
}