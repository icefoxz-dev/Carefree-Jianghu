using _Data;
using _Game;
using _Game._Models;
using UniMvc.Views;
using UnityEngine.Events;
using UnityEngine.UI;

public class View_Player : UiBase
{
    private RoleData player => Game.World.Role;
    private View_info view_info { get; }

    public View_Player(IView v, UnityAction onClickAction) : base(v, true)
    {
        view_info = new View_info(v.Get<View>("view_info"), () =>
        {
            onClickAction?.Invoke();
        });
    }
    public void SetInfo() => view_info.SetInfo(player);
    public void SetSkills() => view_info.SetSkills(player);
    public void SetTags() => view_info.SetTags(player);
    public void ShowInfo() => view_info.Show();
    public void HideInfo() => view_info.Hide();
    private class View_info : UiBase
    {
        private View_main view_main { get; set; }
        private View_tags view_tags { get; }
        private View_skills view_skills { get; }
        private Button btn_nextRound { get; }

        public View_info(IView v, UnityAction onClickAction) : base(v, true)
        {
            view_main = new View_main(v.Get<View>("view_main"));
            view_tags = new View_tags(v.Get<View>("view_tags"));
            view_skills = new View_skills(v.Get<View>("view_skills"));
            btn_nextRound = v.Get<Button>("btn_nextRound");
            btn_nextRound.onClick.AddListener(onClickAction);
        }
        public void SetInfo(RoleData player) => view_main.SetName($"{player}");
        public void SetSkills(RoleData player) => view_skills.SetSkill(player.Skill, "技能");
        public void SetTags(RoleData player) => view_tags.SetTags(player);
        private class View_main : UiBase
        {
            private Image img_avatar { get; }
            private Text text_name { get; }

            public View_main(IView v, bool display = true) : base(v, display)
            {
                img_avatar = v.Get<Image>("img_avatar");
                text_name = v.Get<Text>("text_name");
            }
            public void SetName(string name) => text_name.text = name;
        }
        private class View_tags : UiBase
        {
            private Text text_power { get; }
            private Text text_wisdom { get; set; }
            private Text text_strength { get; set; }
            private Text text_intelligent { get; set; }
            private Text text_silver { get; set; }
            private Text text_stamina { get; set; }
            public View_tags(IView v, bool display = true) : base(v, display)
            {
                text_power = v.Get<Text>("text_power");
                text_wisdom = v.Get<Text>("text_wisdom");
                text_strength = v.Get<Text>("text_strength");
                text_intelligent = v.Get<Text>("text_intelligent");
                text_silver = v.Get<Text>("text_silver");
                text_stamina = v.Get<Text>("text_stamina");
            }
            private void SetPower(double power) => text_power.text = power.ToString();
            private void SetWisdom(double wisdom) => text_wisdom.text = wisdom.ToString();
            private void SetStrength(double strength) => text_strength.text = strength.ToString();
            private void SetIntelligent(double intelligent) => text_intelligent.text = intelligent.ToString();
            private void SetSilver(double silver) => text_silver.text = silver.ToString();
            private void SetStamina(double stamina) => text_stamina.text = stamina.ToString();
            public void SetTags(RoleData player)
            {
                SetPower(player.Power);
                SetWisdom(player.Wisdom);
                SetStrength(player.Strength);
                SetIntelligent(player.Intelligent);
                SetSilver(player.Silver);
                SetStamina(player.Stamina);
            }
        }
        private class View_skills : UiBase
        {
            private ListView_Trans<Prefab_Skill> SkillsListView { get; }

            public View_skills(IView v, bool display = true) : base(v, display)
            {
                SkillsListView = new ListView_Trans<Prefab_Skill>(v, "prefab_skill", "layout_tags");
            }

            public void SetSkill(ISkillTagManager skill, string tagName)
            {
                SkillsListView.ClearList(u => u.Destroy());
                foreach (var tag in skill.Set)
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

}
