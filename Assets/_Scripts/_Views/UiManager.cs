using _Views.Cursor;
using _Views.MainPage;
using _Views.StoryPage;
using UniMvc.Core;
using UniMvc.Views;
using UnityEngine;

namespace _Views
{
    public class UiManager : UiManagerBase
    {
        [SerializeField] private View page_storyView;
        private Page_Story page_story { get; set; }
        [SerializeField] private View page_mainView;
        private Page_Main page_main { get; set; }
        [SerializeField] private View cursor_uiView;
        private Cursor_Ui cursor_ui { get; set; }
        [SerializeField] private View window_confirmView;

        public override void Init()
        {
            page_story = new Page_Story(page_storyView);
            page_main = new Page_Main(page_mainView, true);
            cursor_ui = new Cursor_Ui(cursor_uiView, false);
        }
    }
}
