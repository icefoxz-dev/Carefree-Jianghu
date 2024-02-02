using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Data
{
    /// <summary>
    /// 场合静态类，定义各种规范和工具
    /// </summary>
    public static class Occasion
    {
        public enum Modes
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
        /// <summary>
        /// 场景阶段，开始，过渡，结束
        /// </summary>
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
    /// 场合交互接口
    /// </summary>
    public interface IOccasionInteraction
    {
        /// <summary>
        /// 获取放置信息
        /// </summary>
        /// <returns></returns>
        IRolePlacing[] GetPlacingInfos();
        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        ICharacter GetCharacter(RolePlacing.Index place);
        /// <summary>
        /// 获取交互
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        IRoleInteraction GetRoleInteraction(RolePlacing.Index place);
    }
    /// <summary>
    /// 故事场景, 表示玩家可以互动的具体场景。这是玩家在故事剧情中做出选择和行动的地方，直接影响故事的发展。
    /// </summary>
    public interface IOccasion
    {
        Occasion.Modes Modes { get; }
        IEpisodeNode EpNode { get; }
        string Name { get; }
        string Description { get; }
        IOccasionInteraction Interaction { get; }
        ISceneContent SceneContent { get; }
        string GetLine(RolePlacing.Index role, int index);
    }

    public interface ISceneContent
    {
        Transform Bg { get; }
        GameObject gameObject { get; }
        UnityEvent<RolePlacing.Index, int> OnRoleLineEvent { get; }
        UnityEvent OnEndEvent { get; }
        void Play(int index);
        void SetRole(RolePlacing.Index place, ICharacter character);
        void DisplayPreview(bool display);
        void SetOrientation(RolePlacing.Index place, RolePlacing.Facing selection);
    }
}