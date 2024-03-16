using System;
using _Game._Controllers;
using _Game;
using _Views;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Data;
using _Game._Models;
using UniMvc.Views;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Page_Player : PageUiBase
{
    private enum Pages
    {
        Stat,
        Backpack
    }
    private RoleData player => Game.World.MainRole;
    private View_Player view_player { get; }
    private View_backpack view_backpack { get; }
    private View_Pages view_pages { get; }
    private ChallengeController ChallengeController => Game.GetController<ChallengeController>();
    public Page_Player(IView v, bool display = false) : base(v, display)
    {
        view_player = new View_Player(v.Get<View>("view_player"), OnRoundConfirm);
        view_backpack = new View_backpack(v.Get<View>("view_backpack"), index => UiManager.ShowInfo($"选中物品=[{index}]"));
        view_pages = new View_Pages(v.Get<View>("view_pages"),
            PlayerStat_Show,
            PlayerBackpack_Show);
        Game.RegEvent(GameEvent.Round_Puepose_Update, b => UpdateOccasion());
    }

    private void OnRoundConfirm()
    {
        var excluded = Game.World.Round.GetExcludedTerms(player);
        if (excluded.Any())
        {
            var excludedText = excluded.Aggregate(string.Empty, (current, term) => current + (term + "\n"));
            UiManager.ShowInfo($"条件不允许：\n{excludedText}");
            Game.World.DebugInfo(Game.World.MainRole);
            Debug.Log("<color=yellow>不能执行下个回合</color>，条件：" + excludedText);
            return;
        }
        UiManager.ShowConfirm("确认?", ChallengeController.Instance);
    }

    private void UpdateOccasion()
    {
        view_player.SetInfo();
        view_player.SetTags();
        view_player.SetSkills();
    }
    private void PlayerStat_Show() => ShowPage(Pages.Stat);
    private void PlayerBackpack_Show() => ShowPage(Pages.Backpack);

    private void ShowPage(Pages stat)
    {
        switch (stat)
        {
            case Pages.Stat:
                view_player.ShowInfo();
                view_backpack.Hide();
                break;
            case Pages.Backpack:
                view_player.HideInfo();
                view_backpack.ShowInventory(player.Inventory);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(stat), stat, null);
        }
    }

    private class View_backpack : UiBase
    {
        private ListView_Scroll<Prefab_item> ItemView { get; }
        private event UnityAction<int> OnItemSelected;
        public View_backpack(IView v, UnityAction<int> onItemSelected, bool display = false) : base(v, display)
        {
            ItemView = new ListView_Scroll<Prefab_item>(v, "prefab_item", "scr_item", display);
            OnItemSelected = onItemSelected;
        }

        public void ShowInventory(ITagManager<IGameTag> tm)
        {
            Show();
            var list = tm.Set.Select(t => (default(Sprite), t.Tag.Name)).ToArray();
            SetList(list);
        }

        public void SetList((Sprite icon, string name)[] list)
        {
            ItemView.ClearList(ui => ui.Destroy());
            for (var i = 0; i < list.Length; i++)
            {
                var index = i;
                var(icon, name) = list[i];
                var ui = ItemView.Instance(v => new Prefab_item(v, () => OnItemSelected?.Invoke(index)));
                ui.Set(icon, name);
            }
        }

        public void SetPrefabSelected(int index)
        {
            for (var i = 0; i < ItemView.List.Count; i++)
            {
                var item = ItemView.List[i];
                item.SetSelected(i == index);
            }
        }
        private class Prefab_item : UiBase
        {
            private Image img_selected { get; }
            private Image img_icon { get; }
            private Text text_name { get; }
            private Button btn_item { get; }
            public Prefab_item(IView v, UnityAction onClickAction) : base(v, true)
            {
                img_selected = v.Get<Image>("img_selected");
                img_icon = v.Get<Image>("img_icon");
                text_name = v.Get<Text>("text_name");
                btn_item = v.Get<Button>("btn_item");
                btn_item.onClick.AddListener(onClickAction);
            }
            public void Set(Sprite icon, string name)
            {
                img_icon.sprite = icon;
                img_icon.gameObject.SetActive(img_icon.sprite);
                text_name.text = name;
            }
            public void SetSelected(bool selected) => img_selected.gameObject.SetActive(selected);
        }
    }
}
