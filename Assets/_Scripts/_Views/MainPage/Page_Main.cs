using _Game;
using _Game._Controllers;
using UniMvc.Views;
using UnityEngine;
using UnityEngine.UI;

namespace _Views.MainPage
{
    public class Page_Main : PageUiBase
    {
        private Button btn_gameStart { get; }
        private MainController Main => Game.GetController<MainController>();

        public Page_Main(IView v,bool display = false) : base(v, display)
        {
            btn_gameStart = v.Get<Button>("btn_gameStart");
            btn_gameStart.onClick.AddListener(StartGame);
        }

        private void StartGame()
        {
            Main.StartGame();
            Hide();
        }
    }
}