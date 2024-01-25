using System;
using _Views.Cursor;
using UniMvc.Utls;
using UniMvc.Views;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Views.StoryPage
{
    public class View_RoleSelector : UiBase
    {
        private ListView_Scroll<Prefab_Role> RoleListView { get; }
        private event UnityAction<PointerEventData, DragHelper.DragEvent, int> OnDrag;

        public View_RoleSelector(IView v,UnityAction<PointerEventData,DragHelper.DragEvent, int> onDragEvent, bool display = true) : 
            base(v, display)
        {
            OnDrag = onDragEvent;
            RoleListView = new ListView_Scroll<Prefab_Role>(v, "prefab_role", "scroll_roles");
        }

        public void SetRoles((string name, string description)[] arg)
        {
            RoleListView.ClearList(u => u.Destroy());
            for (var i = 0; i < arg.Length; i++)
            {
                var (name, description) = arg[i];
                var index = i;
                var ui = RoleListView.Instance(v => new Prefab_Role(v, (p, e) => OnDrag?.Invoke(p, e, index)));
                ui.Set(name, description);
            }
        }

        //methods: Set
        private class Prefab_Role : UiBase
        {
            private Image img_avatar { get; }
            private Text text_name { get; }
            private Text text_description { get; }
            private UiDragHandler dragHandler { get; }

            public Prefab_Role(IView v, UnityAction<PointerEventData, DragHelper.DragEvent> onDrag, bool display = true) 
                : base(v, display)
            {
                img_avatar = v.Get<Image>("img_avatar");
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

            public void SetAvatar(Sprite avatar)
            {
                img_avatar.sprite = avatar;
            }
        }
    }
}