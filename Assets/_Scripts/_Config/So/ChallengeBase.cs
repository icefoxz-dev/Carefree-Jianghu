using _Data;

namespace _Config.So
{
    /// <summary>
    /// 挑战基类，主要是用于战斗或小游戏设定配置
    /// </summary>
    public abstract class ChallengeArgsBase : AutoUnderscoreNamingObject, IChallengeArgs
    {
        public abstract ChallengeTypes ChallengeType { get; }
    }
}