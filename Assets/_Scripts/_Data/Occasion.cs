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
    
    /// <summary>
    /// 故事场景, 表示玩家可以互动的具体场景。这是玩家在故事剧情中做出选择和行动的地方，直接影响故事的发展。
    /// </summary>
    public interface IOccasion
    {
        Occasion.Modes Mode { get; }
        string Name { get; }
        string Description { get; }
        //ISceneContent SceneContent { get; }
        /// <summary>
        /// 获取放置信息
        /// </summary>
        /// <returns></returns>
        IRolePlacing[] GetPlacingInfos();
        string GetLine(RolePlacing.Index role, int index);
        IFuncTag[] Results { get; }
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
}