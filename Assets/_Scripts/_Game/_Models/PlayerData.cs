using System.Collections.Generic;
using System.Linq;
using _Data;
using UnityEngine;

namespace _Game._Models
{
    /// <summary>
    /// 玩家信息，包括玩家的角色信息和玩家的游戏数据。
    /// </summary>
    public partial class PlayerData 
    {
        //这里的是一些属性参考代码，为了方便阅读，我把它们放在了一起
        public double Strength => _attributeMap.GetStrength(this);
        public double Intelligent => _attributeMap.GetIntelligent(this);
        public double Power => _attributeMap.GetPower(this);
        public double Wisdom => _attributeMap.GetWisdom(this);
        public double Silver => _attributeMap.GetSilver(this);
        public double Stamina => _attributeMap.GetStamina(this);

        public int Id => _character.Id;
        public string Name => _character.Name;
        public string Description => _character.Description;
        public GameObject Prefab => _character.Prefab;

        public override string ToString() => _character.ToString();
    }
    //下面是实现构造函数的代码
    public partial class PlayerData : IRoleData, IRoleAttributes, ICharacter
    {
        private readonly Character _character;
        private readonly TagManager _trait;
        private readonly TagManager _capable;
        private readonly TagManager _skill;
        private readonly StateTagManager _status;
        private readonly TagManager _inventory;
        private readonly ICharacterAttributeMap _attributeMap;

        public PlayerData(IRoleData playerData, ICharacterAttributeMap attributeMap)
        {
            _character = new Character(playerData.Character);
            _trait = new TagManager(playerData.Attributes.Trait);
            _capable = new TagManager(playerData.Attributes.Capable);
            _skill = new TagManager(playerData.Attributes.Skill);
            _status = new StateTagManager(playerData.Attributes.Status.Tags.Cast<IStatusTag>());
            _inventory = new TagManager(playerData.Attributes.Inventory);
            _attributeMap = attributeMap;
        }

        public ITagManager Trait => _trait;
        public ITagManager Capable => _capable;
        public ITagManager Status => _status;
        public ITagManager Skill => _skill;
        public ITagManager Inventory => _inventory;

        public IEnumerable<IValueTag> GetAllTags() => Trait.ConcatTags(Capable, Status, Skill);

        public ICharacter Character => _character;
        public IRoleAttributes Attributes => this;
    }
}