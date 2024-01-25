using System.Collections.Generic;
using _Data;

namespace _Game._Models
{
    public class RolePlay : IRolePlay
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public IReadOnlyList<IPlotTag> Tags { get; }

        public RolePlay(IRolePlay r)
        {
            Id = r.Id;
            Name = r.Name;
            Description = r.Description;
        }
    }
}