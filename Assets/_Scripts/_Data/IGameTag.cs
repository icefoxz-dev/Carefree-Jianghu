using System;

namespace _Data
{
    /// <summary>
    /// 游戏标签。
    /// </summary>
    public interface IGameTag
    {
        string Name { get; }
        ITagManager GetTagManager(IRoleAttributes attributes);
    }
}