using System;
using _Views;
using UniMvc.Views;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Window_Confirm : WinUiBase
{
    private Text text_message { get; }
    private Button btn_confirm { get; }
    private Button btn_cancel { get; }

    public Window_Confirm(IView v, bool display = true) : base(v, display)
    {
        text_message = v.Get<Text>("text_message");
        btn_confirm = v.Get<Button>("btn_confirm");
        btn_cancel = v.Get<Button>("btn_cancel");
    }

    public void Show(string message, UnityAction onConfirm, UnityAction onCancel)
    {
        Display(true);
        text_message.text = message;
        btn_confirm.onClick.AddListener(()=>
        {
            onConfirm?.Invoke();
            Hide();
        });
        btn_cancel.onClick.AddListener(()=>
        {
            onCancel?.Invoke();
            Hide();
        });
    }
}
