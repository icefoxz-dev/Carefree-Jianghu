namespace _Data
{
    /// <summary>
    /// 角色标签。
    /// </summary>
    public interface IRoleTag : IGameTag
    {
    }
    /// <summary>
    /// 功能标签，代表游戏中的一个功能标签。每个标签都有一个名称，表示玩家在游戏中的选择和行动。
    /// </summary>
    public interface IFuncTag : IGameTag
    {
        void UpdateRole(IRoleData role);
    }

    /// <summary>
    /// 值标签, 根据标签类型提供值用于判断<see cref="ITagTerm"/>和加法更新<see cITagSet{T}et{T}.AddTagValue"/>
    /// </summary>
    public interface IValueTag : IGameTag
    {
        double Value { get; }
    }


    /// <summary>
    /// 能力标签，不能直接赋值，它是透过公式计算得到的
    /// </summary>
    public interface IFormulaTag : IGameTag
    {
        double GetValue(IRoleData role);
    }
}