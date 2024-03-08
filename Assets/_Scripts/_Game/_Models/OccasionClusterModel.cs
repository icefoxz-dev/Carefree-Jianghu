using System.Collections.Generic;
using _Data;

namespace _Game._Models
{
    public class OccasionClusterModel : ModelBase, IOccasionCluster
    {
        private readonly IPurpose[] _purposes;
        public Character Role { get; private set; }

        public OccasionClusterModel(IPurpose[] purposes)
        {
            _purposes = purposes;
        }

        public IEnumerable<IPurpose> GetPurposes(IRoleData role) => _purposes;

        public void SetRole(Character role)
        {
            Role = role;
        }
    }
}