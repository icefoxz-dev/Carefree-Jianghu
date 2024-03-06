namespace _Data
{
    /// <summary>
    /// 功能标签，代表游戏中的一个功能标签。每个标签都有一个名称，表示玩家在游戏中的选择和行动。
    /// </summary>
    public interface IFuncTag
    {
        /// <summary>
        /// 判断使用的标签(so)
        /// </summary>
        IGameTag GameTag { get; }
        string Name => GameTag.Name;
        void UpdateRole(IRoleData role);
    }

    /// <summary>
    /// 值标签, 根据标签类型提供值用于判断<see cref="IPlotTerm"/>和加法更新<see cref="ITagManager.AddTagValue"/>
    /// </summary>
    public interface IValueTag 
    {
        /// <summary>
        /// 判断使用的标签(so)
        /// </summary>
        IGameTag GameTag { get; }//为了获取so标签作为判断
        string Name => GameTag.Name;
        double Value { get; }
    }

    public static class ValueTagExtension
    {
        /// <summary>
        /// 给玩家添加标签值
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="role"></param>
        public static void UpdateRole(this IValueTag tag, IRoleData role)
        {
            var manager = tag.GameTag.GetTagManager(role.Attributes);
            manager.AddTagValue(tag);
        }
    }
}