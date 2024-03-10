using System;
using System.Collections.Generic;
using System.Linq;
using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "CharacterAttributeMapSo", menuName = "配置/设定/人物属性映象")]
    public class CharacterTagsMapSo : ScriptableObject,ICharacterTagsMap
    {
        [SerializeField] private AbilitySo 武力;
        [SerializeField] private AbilitySo 才智;
        [SerializeField] private AbilitySo 力量;
        [SerializeField] private AbilitySo 智力;
        [SerializeField] private StatusTagSo 体力;
        [SerializeField] private AbilitySo 银两;
        [SerializeField] private AbilitySo 声望;
        [SerializeField] private StoryTagSo 游戏结束;

        public IRoleTag Strength => 力量;
        public IRoleTag Intelligent => 智力;
        public IRoleTag Power => 武力;
        public IRoleTag Wisdom => 才智;
        public IStatusTag Stamina => 体力.GetStatusTag();
        public IRoleTag Silver => 银两;
        public IRoleTag Reputation => 声望;
        public IRoleTag RoleOver => 游戏结束;
        public IEnumerable<ICapableTag> GetCapableTags => Array.Empty<ICapableTag>();//todo 暂时这样实现。后续需要重构Tag才可以

        public double GetStrength(IRoleData player) => player.Attributes.Ability.GetTagValue(Strength);
        public double GetIntelligent(IRoleData player) => player.Attributes.Ability.GetTagValue(Intelligent);
        public double GetPower(IRoleData player) => player.Attributes.Ability.GetTagValue(Power);
        public double GetWisdom(IRoleData player) => player.Attributes.Ability.GetTagValue(Wisdom);
        public double GetSilver(IRoleData player) => player.Attributes.Ability.GetTagValue(Silver);
        public double GetStamina(IRoleData player) => player.Attributes.Status.GetTagValue(Stamina.Tag);
        public double GetReputation(IRoleData player) => player.Attributes.Ability.GetTagValue(Reputation);
        public bool IsGameOver(IRoleData player) => player.Attributes.Story.Tags.Any(t => t.Tag == RoleOver);
    }
}