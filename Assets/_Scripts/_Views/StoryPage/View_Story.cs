using System;
using _Data;
using _Game;
using _Game._Models;
using UniMvc.Views;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Views.StoryPage
{
    public class View_Story : UiBase
    {
        private readonly Element_Occasion[] _occasions = new Element_Occasion[6];
        public View_Story(IView v,UnityAction<(int occasionIndex, RolePlacing.Index roleIndex)> onRoleClickAction, bool display = true) : base(v, display)
        {
            for (var i = 0; i < _occasions.Length; i++)
            {
                var occasionIndex = i;
                _occasions[i] = new Element_Occasion(v.Get<View>($"element_occasion_{i + 1}"), r => onRoleClickAction((occasionIndex, r)));
            }
        }

        //Methods:
        //public void PlaceOccasion(Vector2 pointerPosition)
        //{
        //    for (var index = 0; index < _occasions.Length; index++)
        //    {
        //        var o = _occasions[index];
        //        if (!RectTransformUtility.RectangleContainsScreenPoint(o.RectTransform, pointerPosition)) continue;
        //        var oc = Game.World.CurrentEp.Occasions[index];
        //        o.Set(oc);
        //        return;
        //    }
        //    throw new NullReferenceException($"No occasion set!");
        //}
        public void SetEpisode(EpisodeBase ep)
        {
            for (var i = 0; i < _occasions.Length; i++)
            {
                IOccasion oc = null;
                if (i < ep.FrameMap.Count)
                    oc = ep.FrameMap[i];
                _occasions[i].Set(oc);
            }
        }
        public void OnOccasionUpdate(int occasionIndex)
        {
            var oc = Game.World.CurrentEp.FrameMap[occasionIndex];
            _occasions[occasionIndex].Set(oc);
        }

        //当拖动角色时检查是否在框内
        public (int occasionIndex, int placeIndex) UpdateOnRoleDrag(PointerEventData pointerEventData)
        {
            for (var index = 0; index < _occasions.Length; index++)
            {
                var oc = _occasions[index];
                var (isInFrame, placeIndex) = oc.UpdateFrame(pointerEventData);
                if (isInFrame) return (index, placeIndex);
            }
            return (-1, -1);
        }

        //SetRole ? todo : 设定角色方法
        private class Element_Occasion : UiBase
        {
            private Modes _mode;

            private enum Modes
            {
                None,
                Versus,
                Solo,
            }
            private Element_Role element_role_left { get; }
            private Element_Role element_role_right { get; }
            private Element_Role element_role_solo { get; }
            private View_Frame view_frame { get; }
            private Image img_pic { get; }
            private Text text_brief { get; }

            public Element_Occasion(IView v, UnityAction<RolePlacing.Index> onRoleClickAction, bool display = false) : base(v, display)
            {
                element_role_left = new Element_Role(v.Get<View>("element_role_left"), RolePlacing.Index.Left, onRoleClickAction);
                element_role_right = new Element_Role(v.Get<View>("element_role_right"), RolePlacing.Index.Right, onRoleClickAction);
                element_role_solo = new Element_Role(v.Get<View>("element_role_solo"), RolePlacing.Index.Solo, onRoleClickAction);
                view_frame = new View_Frame(v.Get<View>("view_frame"));
                img_pic = v.Get<Image>("img_pic");
                text_brief = v.Get<Text>("text_brief");
                SetMode(Modes.None);
            }

            public (bool isInFrame, int placeIndex) UpdateFrame(PointerEventData pointerEventData)
            {
                // 检查点是否在 RectTransform 内
                bool isInFrame = RectTransformUtility.RectangleContainsScreenPoint(RectTransform, pointerEventData.position, Game.MainCamera);
                //var isInFrame = RectTransformUtility.RectangleContainsScreenPoint(RectTransform, pointerEventData.position);
                var roles = GetRolesByMode();
                for (var index = 0; index < roles.Length; index++)
                {
                    var role = roles[index];
                    var isInRoleFrame =
                        RectTransformUtility.RectangleContainsScreenPoint(role.RectTransform, pointerEventData.position, Game.MainCamera);
                    //RectTransformUtility.RectangleContainsScreenPoint(role.RectTransform, pointerEventData.position);
                    //Debug.Log($"{GameObject.name}.UpdateFrame: IsInFrame={isInFrame}, {role.GameObject.name} isInFrame = {isInRoleFrame}");
                    role.SetSelected(isInRoleFrame && isInFrame);
                    if (isInRoleFrame && isInFrame) return (true, (int)role.PlaceIndex);
                }
                return (isInFrame, -1);
            }

            private Element_Role[] GetRolesByMode()
            {
                return _mode switch
                {
                    Modes.None => Array.Empty<Element_Role>(),
                    Modes.Versus => new[] { element_role_left, element_role_right },
                    Modes.Solo => new[] { element_role_solo },
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            public void Set(IOccasion? oc)
            {
                var mode = oc?.Modes switch
                {
                    Occasion.Modes.Versus => Modes.Versus,
                    Occasion.Modes.Solo => Modes.Solo,
                    _ => Modes.None
                };
                text_brief.text = oc?.Description ?? string.Empty;
                SetMode(mode);
                SetRoles(oc);
                Show();
                return;

                void SetRoles(IOccasion o)
                {
                    switch (_mode)
                    {
                        case Modes.None:
                            break;
                        case Modes.Versus:
                        {
                            var leftChar = o.Interaction.GetCharacter(RolePlacing.Index.Left);
                            var rightChar = o.Interaction.GetCharacter(RolePlacing.Index.Right);
                            element_role_left.Set(leftChar);
                            element_role_right.Set(rightChar);
                            break;
                        }
                        case Modes.Solo:
                        {
                            var soloChar = o.Interaction.GetCharacter(RolePlacing.Index.Solo);
                            element_role_solo.Set(soloChar);
                            break;
                        }
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            private void SetMode(Modes mode)
            {
                _mode = mode;
                element_role_left.Display(mode == Modes.Versus);
                element_role_right.Display(mode == Modes.Versus);
                element_role_solo.Display(mode == Modes.Solo);
            }

            //Methods: Set(Role), SetSelected
            private class Element_Role : UiBase
            {
                private Prefab_RolePlay prefab_rolePlay { get; }
                private Image img_selected { get; }
                public RolePlacing.Index PlaceIndex { get; }

                public Element_Role(IView v, RolePlacing.Index index ,UnityAction<RolePlacing.Index> onclickAction, bool display = false) : base(v, display)
                {
                    PlaceIndex = index;
                    prefab_rolePlay = new Prefab_RolePlay(v.Get<View>("prefab_rolePlay"), () => onclickAction(PlaceIndex));
                    img_selected = v.Get<Image>("img_selected");
                }

                public void Set(ICharacter character)
                {
                    if(character is null)
                    {
                        prefab_rolePlay.ResetUi();
                        return;
                    }
                    prefab_rolePlay.Set(character);
                }

                public void SetSelected(bool selected) => img_selected.gameObject.SetActive(selected);

                //Methods: Set(Role)
                private class Prefab_RolePlay : UiBase
                {
                    private Element_text element_text_header { get; }
                    private Element_text element_text_name { get; }
                    private Element_text element_text_message { get; }
                    private Button btn_click { get; }

                    public Prefab_RolePlay(IView v, UnityAction onclickAction, bool display = true) : base(v, display)
                    {
                        element_text_header = new Element_text(v.Get<View>("element_text_header"));
                        element_text_name = new Element_text(v.Get<View>("element_text_name"));
                        element_text_message = new Element_text(v.Get<View>("element_text_message"));
                        btn_click = v.Get<Button>("btn_click");
                        btn_click.onClick.AddListener(onclickAction);
                    }

                    public void Set(ICharacter character)
                    {
                        element_text_header.Set(string.Empty);
                        element_text_name.Set(character.Name);
                        element_text_message.Set(character.Description);
                    }

                    public override void ResetUi()
                    {
                        base.ResetUi();
                        element_text_header.Set(string.Empty);
                        element_text_name.Set(string.Empty);
                        element_text_message.Set(string.Empty);
                    }

                    //Methods: Set(text)
                    private class Element_text : UiBase
                    {
                        private Text text_value { get; }
                        public Element_text(IView v, bool display = true) : base(v, display)
                        {
                            text_value = v.Get<Text>("text_value");
                        }

                        public void Set(string text)=> text_value.text = text;
                    }
                }
            }

            //Methods: SetTheme
            private class View_Frame : UiBase
            {
                public enum Themes
                {
                    None,
                    Primary,
                    Secondary,
                    Success,
                    Warning,
                    Failed,
                }
                private Image img_primary { get; }
                private Image img_secondary { get; }
                private Image img_success { get; }
                private Image img_warning { get; }
                private Image img_failed { get; }

                public View_Frame(IView v, bool display = true) : base(v, display)
                {
                    img_primary = v.Get<Image>("img_primary");
                    img_secondary = v.Get<Image>("img_secondary");
                    img_success = v.Get<Image>("img_success");
                    img_warning = v.Get<Image>("img_warning");
                    img_failed = v.Get<Image>("img_failed");
                    SetTheme(Themes.None);
                }

                public void SetTheme(Themes theme)
                {
                    img_primary.gameObject.SetActive(theme == Themes.Primary);
                    img_secondary.gameObject.SetActive(theme == Themes.Secondary);
                    img_success.gameObject.SetActive(theme == Themes.Success);
                    img_warning.gameObject.SetActive(theme == Themes.Warning);
                    img_failed.gameObject.SetActive(theme == Themes.Failed);
                }
            }
        }
    }
}