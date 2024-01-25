using System;
using System.Collections.Generic;
using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "CharacterSo", menuName = "配置/角色/人物")]
    public class RoleSo : AutoUnderscoreNamingObject
    {
        [SerializeField] [TextArea] private string _description;
        [SerializeField] private RoleTrait[] _traits;
        public IRolePlay GetRole() => new RolePlay(Id, Name, _description, _traits);

        private class RolePlay : IRolePlay
        {
            public int Id { get; }
            public IReadOnlyList<IPlotTag> Tags { get; }
            public string Name { get; }
            public string Description { get; }

            public RolePlay(int id, string name, string description, RoleTrait[] traits)
            {
                Id = id;
                Name = name;
                Description = description;
                Tags = traits;
            }
        }

        [Serializable]
        private class RoleTrait : IPlotTag
        {
            public TraitSo _so;
            [Min(1)] public double _value;
            public string Name => _so.name;
            public double Value => _value;
        }
    }
}