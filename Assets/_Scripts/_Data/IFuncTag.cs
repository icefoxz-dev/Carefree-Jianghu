namespace _Data
{
    /// <summary>
    /// 功能标签，代表游戏中的一个功能标签。每个标签都有一个名称，表示玩家在游戏中的选择和行动。
    /// </summary>
    public interface IFuncTag 
    {
        IGameTag GameTag { get; }
        string Name => GameTag.Name;
        double Value { get; }
    }

    public static class FuncTagExtension
    {
        /// <summary>
        /// 给玩家添加标签值
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="player"></param>
        public static void SetPlayer(this IFuncTag tag, IRoleData player)
        {
            var manager = tag.GameTag.GetTagManager(player.Prop);
            manager.AddTagValue(tag);
        }
    }
}