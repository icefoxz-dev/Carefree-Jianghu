using System;
using System.Collections.Generic;

namespace _Data
{
    /// <summary>
    /// 角色静态类，定义各种规范和工具
    /// </summary>
    public static class Role
    {
        public enum Index
        {
            Solo = 0,
            Left = 1,
            Right = 2,
        }
        public enum InteractionType
        {
            Main,
            Team,
            Fixed,
        }

        public enum Facing
        {
            Left,
            Right,
        }
    }

    public interface IRoleTerm
    {
        int Id { get; }
        IReadOnlyList<IPlotTag> Tags { get; }
    }

    public interface IRolePlay : IRoleTerm
    {
        string Name { get; }
        string Description { get; }
    }

    [Serializable]
    public record RolePlayData(int Id, string Name, string Description, IReadOnlyList<IPlotTag> Tags) : IRolePlay
    {
        public int Id { get; } = Id;
        public IReadOnlyList<IPlotTag> Tags { get; } = Tags;
        public string Description { get; } = Description;
        public string Name { get; } = Name;
    }
}