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
        private View_CardSelector view_cardSelector { get; }
        private StoryController StoryController => Game.GetController<StoryController>();
        public Page_Story(IView v, bool display = true) : base(v, display)
        {
            view_story = new View_Story(v.Get<View>("view_story"), (a) => OnOccasionRoleClick(a.occasionIndex, a.roleIndex));
            view_cardSelector = new View_CardSelector(v.Get<View>("view_cardSelector"), OnRoleDragEvent);
            Game.RegEvent(GameEvent.Episode_Start, b => LoadLastEpisode());
            Game.RegEvent(GameEvent.Episode_Occasion_Update, b => view_story.OnOccasionUpdate(b.Get<int>(0)));
        }

        private void OnRoleDragEvent((PointerEventData p, DragHelper.DragEvent e,int avatarIndex ,int index) arg)
        {
            var (p, e, avatarIndex, index) = arg;
            //var isInScene = Game.Scene.ComicScene.IsInScene(p);
            //var text = isInScene?"<color=cyan>InScene</color>" : "<color=red>OutScene</color>";
            //Debug.Log("Comic Scene: " + text);
            switch (e)
            {
                case DragHelper.DragEvent.Begin:
                {
                    var title = avatarIndex ==0? Game.World.Team[index].Name : Game.World.Occasions[index];
                    Cursor_Ui.Set(title);
                    break;
                }
                case DragHelper.DragEvent.Dragging:
                {
                    var (occasionIndex, roleIndex) = view_story.UpdateOnRoleDrag(p);
                    Cursor_Ui.SetPosition(p.position);
                    break;
                }
                case DragHelper.DragEvent.End:
                {
                    Cursor_Ui.Hide();
                    var (occasionIndex, placeIndex) = view_story.UpdateOnRoleDrag(p);
                    if (occasionIndex < 0 || placeIndex < 0) return;
                    if (avatarIndex == 1)
                    {
                        Debug.LogError("暂时不支持场景。");
                        return;
                    }
                    StoryController.PlaceRoleToOccasion(index, (RolePlacing.Index)placeIndex, occasionIndex);
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
            var occasion = Game.World.Occasions;
            view_story.SetEpisode(ep);
            var options = team.Select(t => (t.Name, t.Description, 0))
                .Concat(occasion.Select(o => (o, o, 1)))
                .ToArray();
            view_cardSelector.SetCards(options);
        }


        private void OnOccasionRoleClick(int occasionIndex, RolePlacing.Index rolePlace)
        {
            //角色点击事件
            //occasionIndex: 0-5, roleIndex: 0=solo, 1=left, 2=right
        
        }
    }
}