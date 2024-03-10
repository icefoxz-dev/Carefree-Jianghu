using System;
using System.Collections.Generic;
using System.Linq;
using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "OccasionPoolSo", menuName = "配置/场合/池")]
    public class OccasionPoolSo : OccasionClusterSoBase, IPurpose
    {
        [SerializeField] private OptionField[] _occasions;
        [SerializeField, TextArea] private string _description;

        public string Description => _description;
        public bool IsMandatory => false;

        public IOccasion GetOccasion(IRoleData role) => _occasions
            .Where(o => o.So.IsInTerm(role))
            .SelectWeightedRandom(1)
            .Select(o => o.So)
            .FirstOrDefault();

        protected override IEnumerable<IPurpose> GetOccasionPurpose(IRoleData role, IGameRound round) =>
            new IPurpose[] { this };

        [Serializable]
        private class OptionField : IWeightElement
        {
            [SerializeField] private OccasionBase _so;
            [SerializeField] private int _weight = 1;
            public double Weight => _weight;
            public IOccasion So => _so;
        }
    }
}