using System;
using System.Linq;
using _Data;
using _Game;
using _Game._Controllers;
using _Views.Cursor;
using UniMvc.Views;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Views.StoryPage
{
    public class Page_Story : PageUiBase
    {
        private View_Story view_story { get; }
        private View_RoleSelector view_roleSelector { get; }
        private StoryController StoryController => Game.GetController<StoryController>();
        public Page_Story(IView v, bool display = true) : base(v, display)
        {
            view_story = new View_Story(v.Get<View>("view_story"), (a) => OnOccasionRoleClick(a.occasionIndex, a.roleIndex));
            view_roleSelector = new View_RoleSelector(v.Get<View>("view_roleSelector"), OnRoleDragEvent);
            Game.RegEvent(GameEvent.Episode_Start, b => LoadLastEpisode());
            Game.RegEvent(GameEvent.Episode_Occasion_Update, b => view_story.OnOccasionUpdate(b.Get<int>(0)));
        }

        private void OnRoleDragEvent(PointerEventData p, DragHelper.DragEvent e, int teamIndex)
        {
            switch (e)
            {
                case DragHelper.DragEvent.Begin:
                {
                    var role = Game.World.Team[teamIndex];
                    Cursor_SelectedRole.Set(role.Name);
                    break;
                }
                case DragHelper.DragEvent.Dragging:
                {
                    var (occasionIndex, roleIndex) = view_story.UpdateOnRoleDrag(p);
                    Cursor_SelectedRole.SetPosition(p.position);
                    break;
                }
                case DragHelper.DragEvent.End:
                {
                    Cursor_SelectedRole.Hide();
                    var (occasionIndex, placeIndex) = view_story.UpdateOnRoleDrag(p);
                    if (occasionIndex < 0 || placeIndex < 0) return;
                    StoryController.PlaceRoleToOccasion(teamIndex, (RolePlacing.Index)placeIndex, occasionIndex);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(e), e, null);
            }
        }

        private void LoadLastEpisode()
        {
            LogEvent();
            var ep = Game.World.CurrentEp;
            var team = Game.World.Team;
            view_story.SetEpisode(ep);
            view_roleSelector.SetRoles(team.Select(t => (t.Name, t.Description)).ToArray());
        }


        private void OnOccasionRoleClick(int occasionIndex, RolePlacing.Index rolePlace)
        {
            //角色点击事件
            //occasionIndex: 0-5, roleIndex: 0=solo, 1=left, 2=right
        
        }
    }
}