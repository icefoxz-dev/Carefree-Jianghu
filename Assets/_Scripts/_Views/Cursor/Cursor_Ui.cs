using _Game;
using UniMvc.Views;
using UnityEngine;
using UnityEngine.UI;

namespace _Views.Cursor
{
    public class Cursor_Ui : UiBase
    {
        private Text text_name { get; }
        private static Cursor_Ui instance { get; set; }
        private static RectTransform parentRect { get; set; }
        public Cursor_Ui(IView v, bool display = true) : base(v, display)
        {
            instance = this;
            parentRect = (RectTransform)Transform.parent;
            text_name = v.Get<Text>("text_name");
        }

        public static void Set(string name, bool display = true)
        {
            instance.text_name.text = name;
            instance.Display(display);
        }

        public static void SetPosition(Vector3 pos, bool display = true)
        {
            // 将世界坐标转换为 Canvas 本地坐标
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, pos, Game.MainCamera, out Vector2 localPoint);
            // 更新 UI 元素位置
            instance.Transform.localPosition = localPoint;
            instance.Display(display);
        }

        public static void Hide() => instance.Display(false);
    }
}