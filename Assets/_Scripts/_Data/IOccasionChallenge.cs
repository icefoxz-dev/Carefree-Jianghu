using System.Collections.Generic;
using UnityEngine.Events;

namespace _Data
{
    /// <summary>
    /// 挑战参数
    /// </summary>
    public interface IChallengeArgs
    {
        ChallengeTypes ChallengeType { get; }
    }
    ///// <summary>
    ///// 挑战接口, 作为挑战或战斗的基础接口
    ///// </summary>
    //public interface IChallenge
    //{
    //    IChallengeArgs Args { get; }
    //}
    /// <summary>
    /// 战斗接口
    /// </summary>
    public interface IChallengeBattleArgs : IChallengeArgs
    {
        IRoleData GetOpponent(IEnumerable<IValueTag> capable);
    }

    public enum ChallengeTypes
    {
        None,
        Battle,
        MiniGame,
    }
}