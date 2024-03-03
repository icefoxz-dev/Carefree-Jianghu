using _Data;
using _Game;
using _Game._Models;
using UniMvc.Views;
using UnityEngine.Events;
using UnityEngine.UI;

public class View_Player : UiBase
{
    private PlayerData player => Game.World.Player;
    private View_info view_info { get; }
    private View_tags view_tags { get; }


    private Button btn_nextRound { get; }

    public View_Player(IView v, UnityAction onClickAction) : base(v, true)
    {
        view_info = new View_info(v.Get<View>("view_info"));
        view_tags = new View_tags(v.Get<View>("view_tags"));
        btn_nextRound = v.Get<Button>("btn_nextRound");
        btn_nextRound.onClick.AddListener(onClickAction);
    }
    public void SetInfo() => view_info.SetInfo(player);
    public void SetSkills() => view_tags.SetSkill(player.Skill, " Ù–‘");
    private class View_info : UiBase
    {
        private Text text_name { get; }
        private Text text_power { get; }
        private Text text_wisdom { get; set; }
        private Text text_strength { get; set; }
        private Text text_intelligent { get; set; }
        private Text text_silver { get; set; }
        private Text text_stamina { get; set; }
        public View_info(IView v) : base(v, true)
        {
            text_name = v.Get<Text>("text_name");
            text_power = v.Get<Text>("text_power");
            text_wisdom = v.Get<Text>("text_wisdom");
            text_strength = v.Get<Text>("text_strength");
            text_intelligent = v.Get<Text>("text_intelligent");
            text_silver = v.Get<Text>("text_silver");
            text_stamina = v.Get<Text>("text_stamina");
        }
        private void SetName(string name) => text_name.text = name;
        private void SetPower(double power) => text_power.text = power.ToString();
        private void SetWisdom(double wisdom) => text_wisdom.text = wisdom.ToString();
        private void SetStrength(double strength) => text_strength.text = strength.ToString();
        private void SetIntelligent(double intelligent) => text_intelligent.text = intelligent.ToString();
        private void SetSilver(double silver) => text_silver.text = silver.ToString();
        private void SetStamina(double stamina) => text_stamina.text = stamina.ToString();
        public void SetInfo(PlayerData player)
        {
            SetName($"{player}");
            SetPower(player.Power);
            SetWisdom(player.Wisdom);
            SetStrength(player.Strength);
            SetIntelligent(player.Intelligent);
            SetSilver(player.Silver);
            SetStamina(player.Stamina);
        }
    }

    private class View_tags : UiBase
    {
        private ListView_Trans<Prefab_Skill> SkillsListView { get; }
        public View_tags(IView v, bool display = true) : base(v, display)
        {
            SkillsListView = new ListView_Trans<Prefab_Skill>(v, "prefab_skill", "layout_tags");
        }

        public void SetSkill(ITagManager pTags, string tagName)
        {
            SkillsListView.ClearList(u => u.Destroy());
            foreach (var tag in pTags.Tags)
            {
                var ui = SkillsListView.Instance(v => new Prefab_Skill(v));
                ui.SetTag(tagName);
                ui.SetSkill($"{tag.Name}: {tag.Value}");
            }
        }
        private class Prefab_Skill : UiBase
        {
            private Text tag_name { get; }
            private Text text_skill { get; }

            public Prefab_Skill(IView v) : base(v, true)
            {
                tag_name = v.Get<Text>("tag_name");
                text_skill = v.Get<Text>("text_skill");
            }
            
            public void SetTag(string tagName) => tag_name.text = tagName;
            public void SetSkill(string tagValue) => text_skill.text = tagValue;

        }
    }

}
