using System.Linq;
using _Data;
using _Game._Models;
using UnityEngine;

namespace _Game._Controllers
{
    public class StoryController : ControllerBase
    {
        public void PlaceRoleToOccasion(int teamIndex)
        {
            var role = World.Team[teamIndex];
            //World.CurrentEp.SetOccasion(occasionIndex, placeIndex, role);
            //World.CurrentOccasion.SetRole(role);
        }

        public void SetOccasion(IPurpose purpose)
        {
            World.SetCurrentPurpose(purpose);
        }
    }
}