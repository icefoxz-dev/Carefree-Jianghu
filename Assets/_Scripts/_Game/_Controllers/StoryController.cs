using System.Linq;
using _Data;
using _Game._Models;
using UnityEngine;

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
            var excluded = World.TryProceedRound();
            if (!excluded.Any()) return;
            var excludedText = excluded.Aggregate(string.Empty, (current, term) => current + (term + "\n"));
            Debug.Log("<color=yellow>不能执行下个回合</color>，条件：" + excludedText);
        }
    }
}