using _Data;

namespace _Game._Controllers
{
    public class StoryController : ControllerBase
    {
        public void PlaceRoleToOccasion(int teamIndex,RolePlacing.Index placeIndex,int occasionIndex)
        {
            var role = World.Team[teamIndex];
            World.CurrentEp.SetOccasion(occasionIndex, placeIndex, role);
        }
    }
}