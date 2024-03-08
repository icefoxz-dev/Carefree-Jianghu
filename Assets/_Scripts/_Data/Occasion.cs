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
    }

    /// <summary>
    /// 场景集，作为多个场景的集合，用于解锁和管理场景
    /// </summary>
    public interface IOccasionCluster 
    {
        /// <summary>
        /// 获取可用场合(可操作项目)
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        IEnumerable<IPurpose> GetPurposes(IRoleData role);
    }

    /// <summary>
    /// 场合, 表示玩家可以互动的具体场景。这是玩家在故事剧情中做出选择和行动的地方，直接影响故事的发展。
    /// </summary>
    public interface IOccasion
    {
        //ISceneContent SceneContent { get; }
        /// <summary>
        /// 获取放置信息
        /// </summary>
        /// <returns></returns>
        //IRolePlacing[] GetPlacingInfos();
        //string GetLine(RolePlacing.Index role, int index);
        void UpdateRewards(IRoleData role);
        IPlotTerm[] GetExcludedTerms(IRoleData role);
    }

    /// <summary>
    /// 玩家操作的目的，用于交互和选择
    /// </summary>
    public interface IPurpose 
    {
        string Name { get; }
        string Description { get; }

        IOccasion GetOccasion(IRoleData role);
    }

    public interface ISceneContent
    {
        Transform Bg { get; }
        GameObject gameObject { get; }
        UnityEvent<RolePlacing.Index, string> OnRoleLineEvent { get; }
        UnityEvent OnEndEvent { get; }
        void Play(int index);
        void SetRole(RolePlacing.Index place, ICharacter character);
        void DisplayPreview(bool display);
        void SetOrientation(RolePlacing.Index place, RolePlacing.Facing selection);
    }

    public static class OccasionExtensions
    {
        public static bool IsInTerm(this IOccasion o, IRoleData role) => o.GetExcludedTerms(role).Length == 0;
    }
}