using System;
using System.Linq;
using _Data;

namespace _Game._Models
{
    /// <summary>
    /// 奖励面板，用于记录(上一个)奖励信息。
    /// </summary>
    public class RewardBoard : ModelBase
    {
        public ITagValue[] Rewards { get; private set; }

        public void SetReward(ITagValue[] rewards,IRoleData role)
        {
            Rewards = rewards;
            foreach (var tag in Rewards)
            {
                if (tag?.Tag == null)
                    throw new NullReferenceException($"Rewards = {string.Join(',', rewards.Select(r => r.Tag?.Name))} ,some tag not set!");
                role.Proceed(tag);
            }
            SendEvent(GameEvent.Reward_Update);
        }
    }
}