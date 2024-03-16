using System;
using System.Linq;
using _Data;
using _Game;
using _Game._Controllers;
using _Game._Models;
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
            view_story = new View_Story(v.Get<View>("view_story"), OnOccasionRoleClick);
            view_cardSelector = new View_CardSelector(v.Get<View>("view_cardSelector"), OnRoleDragEvent);
            Game.RegEvent(GameEvent.Round_Puepose_Update, b => UpdateOccasion());
        }

        private void OnRoleDragEvent((PointerEventData p, DragHelper.DragEvent e,int cardType ,int index) arg)
        {
            var (p, e, cardType, index) = arg;
            //var isInScene = Game.Scene.ComicScene.IsInScene(p);
            //var text = isInScene?"<color=cyan>InScene</color>" : "<color=red>OutScene</color>";
            //Debug.Log("Comic Scene: " + text);
            switch (e)
            {
                case DragHelper.DragEvent.Begin:
                {
                    var title = cardType == 0 ? Game.World.Team[index].Name : Game.World.Round.Purposes[index]?.Name;
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
                    var (isInFrame , placeIndex) = view_story.UpdateOnRoleDrag(p);
                    if (!isInFrame) return;
                    if (cardType == 1)
                    {
                        var oc = Game.World.Round.Purposes[index];
                        StoryController.SetOccasion(oc);
                        return;
                    }
                    StoryController.PlaceRoleToOccasion(index);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(e), e, null);
            }
        }

        private void UpdateOccasion()
        {
            LogEvent();
            var world = Game.World;
            var team = world.Team;
            var purposes = world.Round.IsMandatory ? Array.Empty<IPurpose>() : world.Round.Purposes;//如果是强制就不显示可选
            view_story.OnOccasionUpdate();
            var options = team.Select(t => (t.Name, t.Description, 0))
                .Concat(purposes.Select(o => (o.Name, o.Description, 1)))
                .ToArray();
            view_cardSelector.SetCards(options);
        }

        private void OnOccasionRoleClick(RolePlacing.Index rolePlace)
        {
            //角色点击事件
            //roleIndex: 0=solo, 1=left, 2=right
        
        }
    }
}