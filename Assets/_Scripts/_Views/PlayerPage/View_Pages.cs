using UniMvc.Views;
using UnityEngine.Events;
using UnityEngine.UI;

public class View_Pages : UiBase
{
    private Element element_player { get; }
    private Element element_backpack { get; }
    private bool IsActive { get; set; }
    public View_Pages(IView v, UnityAction onPlayerAction, UnityAction onBackpackAction) : base(v, true)
    {
        element_player = new Element(v.Get<View>("element_player"), () =>
        {
            if(IsActive) onPlayerAction?.Invoke();
        });
        element_backpack = new Element(v.Get<View>("element_backpack"), () =>
        {
            if (IsActive) onBackpackAction?.Invoke();
        });
        IsActive = true;
    }
    public void SetActive(bool isActive) => IsActive = isActive;
    private class Element : UiBase
    {
        private Button btn_page { get; }
        public Element(IView v, UnityAction onClickAction) : base(v, true)
        {
            btn_page = v.Get<Button>("btn_page");
            btn_page.onClick.AddListener(onClickAction);
        }
    }

}
