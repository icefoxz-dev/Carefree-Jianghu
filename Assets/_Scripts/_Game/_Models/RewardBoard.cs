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

        public void SetReward(IPurpose purpose,IRoleData role)
        {
            Occasion = purpose.GetOccasion(role);
            Rewards = Occasion.Rewards;
            foreach (var tag in Rewards)
            {
                if (tag == null)
                    throw new NullReferenceException(
                        $"Purpose={purpose.Name} Occasion Rewards = {string.Join(',', Occasion.Rewards.Select(r => r?.Name))} ,game tag not set!");
                tag.UpdateRole(role);
            }
            SendEvent(GameEvent.Reward_Update);
        }
    }
}