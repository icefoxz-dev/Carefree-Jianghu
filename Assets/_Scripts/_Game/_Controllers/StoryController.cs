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

        public void ConfirmRound()
        {
            var excluded = World.TryProceedRound();
            if (!excluded.Any()) return;
            var excludedText = excluded.Aggregate(string.Empty, (current, term) => current + (term + "\n"));
            Debug.Log("<color=yellow>不能执行下个回合</color>，条件：" + excludedText);
            Game.World.DebugInfo(Game.World.Player);
        }
    }
}