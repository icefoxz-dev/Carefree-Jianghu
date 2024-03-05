using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "CharacterAttributeMapSo", menuName = "配置/设定/人物属性映象")]
    public class CharacterAttributeMapSo : ScriptableObject,ICharacterAttributeMap
    {
        [SerializeField] private CapableSo 武力;
        [SerializeField] private CapableSo 才智;
        [SerializeField] private CapableSo 力量;
        [SerializeField] private CapableSo 智力;
        [SerializeField] private StatusTagSo 体力;
        [SerializeField] private CapableSo 银两;
        [SerializeField] private CapableSo 声望;

        public IGameTag Strength => 力量;
        public IGameTag Intelligent => 智力;
        public IGameTag Power => 武力;
        public IGameTag Wisdom => 才智;
        public IStatusTag Stamina => 体力.GetStatusTag();
        public IGameTag Silver => 银两;
        public IGameTag Reputation => 声望;

        public double GetStrength(IRoleData player) => player.Attributes.Capable.GetTagValue(Strength);
        public double GetIntelligent(IRoleData player) => player.Attributes.Capable.GetTagValue(Intelligent);
        public double GetPower(IRoleData player) => player.Attributes.Capable.GetTagValue(Power);
        public double GetWisdom(IRoleData player) => player.Attributes.Capable.GetTagValue(Wisdom);
        public double GetSilver(IRoleData player) => player.Attributes.Capable.GetTagValue(Silver);
        public double GetStamina(IRoleData player) => player.Attributes.Status.GetTagValue(Stamina);
        public double GetReputation(IRoleData player) => player.Attributes.Capable.GetTagValue(Reputation);
    }
}