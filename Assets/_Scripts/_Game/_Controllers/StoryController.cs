using _Data;

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

        public void SetOccasion(IOccasion occasion)
        {
            World.SetCurrentOccasion(occasion);
        }
    }
}