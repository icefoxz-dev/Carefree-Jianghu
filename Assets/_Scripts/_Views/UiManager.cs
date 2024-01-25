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
        [SerializeField] private View cursor_selectedRoleView;
        private Cursor_SelectedRole cursor_selectedRole { get; set; }

        public override void Init()
        {
            page_story = new Page_Story(page_storyView);
            page_main = new Page_Main(page_mainView, true);
            cursor_selectedRole = new Cursor_SelectedRole(cursor_selectedRoleView, false);
        }
    }
}
