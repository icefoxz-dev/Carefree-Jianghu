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
        ////这里的是一些属性参考代码，为了方便阅读，我把它们放在了一起
        public double Strength => this.Strength();
        public double Intelligent => this.Intelligent();
        public double Power => this.Power();
        public double Wisdom => this.Wisdom();
        public double Silver => this.Silver();
        public double Stamina => this.Stamina();

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
        private readonly CapableTagManager _capable;
        private readonly TagManager _trait;
        private readonly TagManager _ability;
        private readonly TagManager _skill;
        private readonly StateTagManager _status;
        private readonly TagManager _inventory;
        private readonly TagManager _episode;

        public PlayerData(IRoleData playerData)
        {
            _character = new Character(playerData.Character);
            _capable = new CapableTagManager(playerData.Attributes.Capable.Tags);
            _trait = new TagManager(playerData.Attributes.Trait);
            _ability = new TagManager(playerData.Attributes.Ability);
            _skill = new TagManager(playerData.Attributes.Skill);
            _status = new StateTagManager(playerData.Attributes.Status.Tags.Cast<IStatusTag>());
            _inventory = new TagManager(playerData.Attributes.Inventory);
            _episode = new TagManager(playerData.Attributes.Story);
        }

        public ITagManager Trait => _trait;
        public ITagManager Ability => _ability;
        public ITagManager Status => _status;
        public ITagManager Skill => _skill;
        public ITagManager Inventory => _inventory;
        public ITagManager Story => _episode;
        public ITagManager Capable => _capable;
        public ICharacter Character => _character;
        public IRoleAttributes Attributes => this;
    }

    public static class RoleDataExtension
    {
        public static double Strength(this IRoleData role) => Game.CharacterTags.GetStrength(role);
        public static double Intelligent(this IRoleData role) => Game.CharacterTags.GetIntelligent(role);
        public static double Power(this IRoleData role) => Game.CharacterTags.GetPower(role);
        public static double Wisdom(this IRoleData role) => Game.CharacterTags.GetWisdom(role);
        public static double Silver(this IRoleData role) => Game.CharacterTags.GetSilver(role);
        public static double Stamina(this IRoleData role) => Game.CharacterTags.GetStamina(role);
    }
}