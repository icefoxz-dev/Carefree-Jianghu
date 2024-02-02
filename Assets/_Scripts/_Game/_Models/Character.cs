using _Data;
using UnityEngine;

namespace _Game._Models
{

    public class Character : ICharacter
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public GameObject Prefab { get; }

        public Character(ICharacter r)
        {
            Id = r.Id;
            Name = r.Name;
            Prefab = r.Prefab;
            Description = r.Description;
        }
    }
}