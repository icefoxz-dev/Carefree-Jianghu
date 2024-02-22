using _Data;
using _Game._Models;

namespace _Game._Controllers
{
    public class StoryController : ControllerBase
    {
        public void PlaceRoleToOccasion(int teamIndex,RolePlacing.Index placeIndex)
        {
            var role = World.Team[teamIndex];
            //World.CurrentEp.SetOccasion(occasionIndex, placeIndex, role);
            World.CurrentOccasion.SetRole(placeIndex, role);
        }

        public void SetOccasion(OccasionModel occasion)
        {
            World.SetCurrentOccasion(occasion);
        }

        public void ConfirmRound()
        {
            World.NextRound();
        }
    }
}