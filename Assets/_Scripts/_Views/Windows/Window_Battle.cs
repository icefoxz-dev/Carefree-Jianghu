using _Data;
using _Game;
using _Game._Controllers;
using _Game._Models;
using UniMvc.Views;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 战斗窗口
/// </summary>
public class Window_Battle : WinUiBase
{
    private Button btn_ok { get; }
    private Text text_message { get; }
    private Text text_btnText { get; }
    private Element_Role element_role_player { get; }
    private Element_Role element_role_opponent { get; }

    private static ChallengeController ChallengeController => Game.GetController<ChallengeController>();
    private Challenge.SimpleBattle Battle => Game.World.Round.Battle;

    public Window_Battle(IView v, bool display = false) : base(v, display)
    {
        btn_ok = v.Get<Button>("btn_ok");
        text_btnText = v.Get<Text>("text_btnText");
        text_message = v.Get<Text>("text_message");
        element_role_player = new Element_Role(v.Get<View>("element_role_player"));
        element_role_opponent = new Element_Role(v.Get<View>("element_role_opponent"));
        Game.MessagingManager.RegEvent(GameEvent.Challenge_Battle_Start, _ => SetBattle(Battle.Player, Battle.Opponent));
    }

    public void SetBattle(IRoleData player, IRoleData opponent)
    {
        btn_ok.onClick.RemoveAllListeners();
        text_message.text = "VS";
        text_btnText.text = "战斗!";
        element_role_opponent.Reset();
        element_role_player.Reset();
        element_role_player.Set(RoleText(player));
        element_role_opponent.Set(RoleText(opponent));
        btn_ok.onClick.AddListener(StartBattle);
        Show();

        void StartBattle()
        {
            ChallengeController.Start();
            var isPlayerWin = Game.World.Round.Battle.IsPlayerWin;
            btn_ok.onClick.RemoveAllListeners();
            text_message.text = isPlayerWin ? "胜利" : "失败";
            element_role_player.SetWin(isPlayerWin);
            element_role_opponent.SetWin(!isPlayerWin);
            text_btnText.text = "确认";
            btn_ok.onClick.AddListener(() =>
            {
                Hide();
                ChallengeController.Finalize();
            });
        }

        string RoleText(IRoleData role) => $"{role.Character.Name}:{role.Power()}";
    }

    private class Element_Role : UiBase
    {
        private Text text_name { get; }
        public Element_Role(IView v, bool display = true) : base(v, display)
        {
            text_name = v.Get<Text>("text_name");
        }

        public void Set(string name)
        {
            text_name.text = name;
        }

        public void Reset()
        {
            text_name.text = "";
            text_name.fontSize = 50;
        }

        public void SetWin(bool isWin) => text_name.fontSize = isWin ? 80 : 40;
    }
}