using System;
using System.Linq;
using _Game;
using _Game._Controllers;
using _Views.Cursor;
using _Views.MainPage;
using _Views.StoryPage;
using UniMvc.Core;
using UniMvc.Utls;
using UniMvc.Views;
using UnityEngine;
using UnityEngine.Events;

namespace _Views
{
    public class UiManager : UiManagerBase
    {
        [SerializeField] private View page_storyView;
        private Page_Story page_story { get; set; }
        [SerializeField] private View page_mainView;
        private Page_Main page_main { get; set; }
        [SerializeField] private View page_playerView;
        private Page_Player page_player { get; set; }
        [SerializeField] private View cursor_uiView;
        private Cursor_Ui cursor_ui { get; set; }
        [SerializeField] private View window_confirmView;
        private static Window_Confirm window_confirm { get; set; }
        [SerializeField] private View window_infoView;
        private static Window_Info window_info { get; set; }
        [SerializeField] private View window_battleView;
        private static Window_Battle window_Battle { get; set; }
        [SerializeField] private View window_introView;
        private static Window_Intro window_intro { get; set; }

        private static void RegEvent(string eventName, Action<DataBag> action) => Game.MessagingManager.RegEvent(eventName, action);

        public override void Init()
        {
            page_story = new Page_Story(page_storyView);
            page_main = new Page_Main(page_mainView, true);
            page_player = new Page_Player(page_playerView, true);
            cursor_ui = new Cursor_Ui(cursor_uiView, false);
            window_confirm = new Window_Confirm(window_confirmView, false);
            window_info = new Window_Info(window_infoView, false);
            window_Battle = new Window_Battle(window_battleView, false);
            window_intro = new Window_Intro(window_introView, false);
            RegEvent(GameEvent.Reward_Update, _ => ShowReward());
            RegEvent(GameEvent.Occasion_Update, _ => PrepareChallenge());
        }

        private void PrepareChallenge() => ShowInfo(Game.World.Round.Occasion.Description,()=>Game.GetController<ChallengeController>().Start());

        private void ShowReward()
        {
            var board = Game.World.RewardBoard;
            if (board.Rewards.Length == 0) return;//0个奖励不显示
            ShowInfo(string.Join('\n', board.Rewards.Select(r => $"{r.Tag.Name}: {r.Value}")));
        }

        public static void ShowConfirm(string message, UnityAction onConfirm = null, UnityAction onCancel = null)
        {
            window_confirm.Show(message, onConfirm, onCancel);
        }
        public static void ShowInfo(string message, UnityAction onClose = null) => window_info.Set(message, onClose);
    }
}
