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
        public IValueTag[] Rewards { get; private set; }

        public IOccasion Occasion { get; private set; }

        public void SetReward(IValueTag[] rewards,IRoleData role,IOccasion occasion)
        {
            Rewards = rewards;
            Occasion = occasion;
            foreach (var tag in Rewards)
            {
                if (tag == null)
                    throw new NullReferenceException(
                        $"Occasion Rewards = {string.Join(',', Occasion.GetRewards(null).Select(r => r?.Name))} ,game tag not set!");
                tag.UpdateRole(role);
            }
            SendEvent(GameEvent.Reward_Update);
        }
    }
}