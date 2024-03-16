using _Game;
using DG.Tweening;
using UniMvc.Views;
using UnityEngine.Events;
using UnityEngine.UI;

public class Window_Intro : WinUiBase
{
    private Text text_intro { get; }
    private Button btn_ok { get; }

    public Window_Intro(IView v, bool display = false) : base(v, display)
    {
        text_intro = v.Get<Text>("text_intro");
        btn_ok = v.Get<Button>("btn_ok");
        Game.MessagingManager.RegEvent(GameEvent.Round_New, _ => Set(Game.World.Round.StoryMap.Intro));
    }

    public void Set(string[] messages, UnityAction onClose = null)
    {
        Display(true);
        btn_ok.gameObject.SetActive(false);
        btn_ok.onClick.RemoveAllListeners();
        btn_ok.onClick.AddListener(() =>
        {
            onClose?.Invoke();
            Hide();
        });
        text_intro.text = string.Empty;
        //dotween 实现 逐字显示 并且在结束的时候显示按钮
        var tween = DOTween.Sequence();
        foreach (var message in messages)
        {
            tween.AppendCallback(() => text_intro.text = string.Empty);
            tween.Append(text_intro.DOText(message, 5f));
            tween.AppendInterval(1f);
        }
        tween.OnComplete(() => btn_ok.gameObject.SetActive(true));
        Show();
    }
}