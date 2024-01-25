namespace _Game._Controllers
{
    public class StoryController : ControllerBase
    {
        public void PlaceRoleToOccasion(int teamIndex,int placeIndex, int occasionIndex)
        {
            var role = World.Team[teamIndex];
            World.CurrentEp.SetOccasion(occasionIndex, placeIndex, role);
        }
    }
}