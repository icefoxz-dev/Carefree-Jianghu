using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Data
{
    public interface IRolePlacing
    {
        RolePlacing.Index Place { get; }
        RolePlacing.Modes Mode { get; }
        ICharacter Character { get; }
    }

    /// <summary>
    /// 角色放置信息类
    /// </summary>
    public class RolePlacing : IRolePlacing
    {
        public enum Index
        {
            Solo = 0,
            Left = 1,
            Right = 2,
        }

        public enum Modes
        {
            /// <summary>
            /// 主角单独出现
            /// </summary>
            Main,
            /// <summary>
            /// 队员可放置
            /// </summary>
            Team,
            /// <summary>
            /// 固定角色
            /// </summary>
            Fixed,
        }

        public enum Facing
        {
            Front,
            Back,
        }

        public Index Place { get; }
        public Modes Mode { get; }
        public ICharacter Character { get; private set; }
        /// <summary>
        /// 交互值，<see cref="Facing"/>将会被转换为<see cref="int"/>，初始为-1表示未选择，如果是Options，将会是选项的索引
        /// </summary>
        public int Selection { get; private set; } = -1;

        public RolePlacing(Index place, Modes mode, ICharacter character)
        {
            Place = place;
            Mode = mode;
            Character = character;
        }
        public RolePlacing(IRolePlacing place)
        {
            Place = place.Place;
            Mode = place.Mode;
            Character = place.Character;
        }

        public void PlaceCharacter(ICharacter character)
        {
            if (Mode == Modes.Fixed)
                throw new Exception($"角色{Character.Name}位置是固定！");
            Character = character;
        }
    }

    /// <summary>
    /// 角色信息接口，主要是定义角色的基本信息
    /// </summary>
    public interface ICharacter
    {
        int Id { get; }
        string Name { get; }
        string Description { get; }
        GameObject Prefab { get; }
    }

    public interface IRoleTerm
    {
        int Id { get; }
        IReadOnlyList<IPlotTag> Tags { get; }
    }
}