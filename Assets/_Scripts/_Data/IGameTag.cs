using System;

namespace _Data
{
    public interface IGameTag
    {
        string Name { get; }
    }
    /// <summary>
    /// 游戏标签。
    /// </summary>
    public interface IRoleTag : IGameTag
    {
        ITagManager GetTagManager(IRoleAttributes attributes);
    }
}