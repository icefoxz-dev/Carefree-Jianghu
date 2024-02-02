using _Game;
using UniMvc.Views;
using UnityEngine;
using UnityEngine.UI;

namespace _Views.Cursor
{
    public class Cursor_SelectedRole : UiBase
    {
        private Text text_name { get; }
        private static Cursor_SelectedRole instance { get; set; }
        public Cursor_SelectedRole(IView v, bool display = true) : base(v, display)
        {
            instance = this;
            text_name = v.Get<Text>("text_name");
        }

        public static void Set(string name, bool display = true)
        {
            instance.text_name.text = name;
            instance.Display(display);
        }

        public static void SetPosition(Vector3 pos, bool display = true)
        {
            var parentRect = instance.Transform.parent as RectTransform;
            // 将世界坐标转换为 Canvas 本地坐标
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, pos, Game.MainCamera, out Vector2 localPoint);

            // 更新 UI 元素位置
            instance.Transform.localPosition = localPoint;
            instance.Display(display);
        }

        public static void Hide() => instance.Display(false);
    }
}