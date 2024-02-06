using System;
using System.Linq;
using _Views.Cursor;
using UniMvc.Utls;
using UniMvc.Views;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Views.StoryPage
{
    public class View_CardSelector : UiBase
    {
        private ListView_Scroll<Prefab_Role> RoleListView { get; }
        private event UnityAction<(PointerEventData pointer, DragHelper.DragEvent dragEvent, int avatarIndex,int index)> OnDrag;

        public View_CardSelector(IView v,UnityAction<(PointerEventData pointer, DragHelper.DragEvent dragEvent, int avatarIndex, int index)> onDragEvent, bool display = true) : 
            base(v, display)
        {
            OnDrag = onDragEvent;
            RoleListView = new ListView_Scroll<Prefab_Role>(v, "prefab_card", "scroll_cards");
        }

        public void SetCards((string name, string description,int avatarIndex)[] arg)
        {
            RoleListView.ClearList(u => u.Destroy());
            var group = arg.GroupBy(a => a.avatarIndex).ToList();
            foreach (var list in group.Select(g => g.ToList()))
            {
                for (var i = 0; i < list.Count; i++)
                {
                    var index = i;
                    var (name, description, avatarIndex) = list[i];
                    var ui = RoleListView.Instance(v =>
                        new Prefab_Role(v, (p, e) => OnDrag?.Invoke((p, e, avatarIndex, index))));
                    ui.Set(name, description);
                    ui.SelectAvatar(avatarIndex);
                }
            }
        }

        //methods: Set, SelectAvatar, SetAvatar
        private class Prefab_Role : UiBase
        {
            private Image img_avatar_1 { get; }
            private Image img_avatar_2 { get; }
            private Text text_name { get; }
            private Text text_description { get; }
            private UiDragHandler dragHandler { get; }

            public Prefab_Role(IView v, UnityAction<PointerEventData, DragHelper.DragEvent> onDrag, bool display = true) 
                : base(v, display)
            {
                img_avatar_1 = v.Get<Image>("img_avatar_1");
                img_avatar_2 = v.Get<Image>("img_avatar_2");
                text_name = v.Get<Text>("text_name");
                text_description = v.Get<Text>("text_description");
                dragHandler = v.GetComponent<UiDragHandler>();
                dragHandler.BeginDrag.AddListener(p => onDrag?.Invoke(p, DragHelper.DragEvent.Begin));
                dragHandler.Drag.AddListener(p => onDrag?.Invoke(p, DragHelper.DragEvent.Dragging));
                dragHandler.EndDrag.AddListener(p => onDrag?.Invoke(p, DragHelper.DragEvent.End));
            }

            public void Set(string name, string description)
            {
                text_name.text = name;
                text_description.text = description;
            }

            public void SelectAvatar(int index)
            {
                img_avatar_1.gameObject.SetActive(index == 0);
                img_avatar_2.gameObject.SetActive(index == 1);
            }

            public void SetAvatar(Sprite avatar)
            {
                img_avatar_1.sprite = avatar;
            }
        }
    }
}