using UniMvc.Views;
using UnityEngine.Events;
using UnityEngine.UI;

public class Window_Info : WinUiBase
{
    private Text text_message { get; }
    private Button btn_ok { get; }

    public Window_Info(IView v, bool display = true) : base(v, display)
    {
        text_message = v.Get<Text>("text_message");
        btn_ok = v.Get<Button>("btn_ok");
    }

    public void Show(string message, UnityAction onClose = null)
    {
        Display(true);
        text_message.text = message;
        btn_ok.onClick.RemoveAllListeners();
        btn_ok.onClick.AddListener(()=>
        {
            onClose?.Invoke();
            Hide();
        });
    }
}