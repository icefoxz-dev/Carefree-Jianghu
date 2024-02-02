using System.Collections.Generic;
using System.Linq;
using _Data;

namespace _Game._Models
{
    public class EpisodeFrame : IOccasion
    {
        /**
         * 1. 场景信息，描述，演示，预制体，动画
         * 2. 场景模式, solo, versus
         * 3. 交互模式, 无, 选项, 方向
         * 4. 角色信息，对话内容
         */

        #region IOccasion
        private readonly IOccasion _ref;
        public Occasion.Modes Modes => _ref.Modes;
        public IEpisodeNode EpNode => _ref.EpNode;
        public string Name => _ref.Name;
        public string Description => _ref.Description;
        public IOccasionInteraction Interaction => _ref.Interaction;

        public string GetLine(RolePlacing.Index role, int index) => _ref.GetLine(role, index);
        #endregion

        private readonly List<RolePlacing> _characters;
        public IReadOnlyList<RolePlacing> Characters => _characters;
        public ISceneContent SceneContent => SceneFrame.CurrentScene;
        public SceneFrame SceneFrame { get; }
        public int FrameIndex { get; }

        public EpisodeFrame(IOccasion o, int frameIndex)
        {
            _ref = o;
            FrameIndex = frameIndex;
            SceneFrame = Game.Scene.GetFrame(frameIndex);
            SceneFrame.SetSceneContent(o.SceneContent);
            _characters = o.Interaction.GetPlacingInfos().Select(p => new RolePlacing(p)).ToList();
        }

        RolePlacing GetRolePlacing(RolePlacing.Index place)
        {
            var rolePlacing = _characters.FirstOrDefault(p => p.Place == place);
            return rolePlacing;
        }

        // 放置方法
        public void PlaceCharacter(RolePlacing.Index place, ICharacter character)
        {
            var rolePlacing = GetRolePlacing(place);
            rolePlacing.PlaceCharacter(character);
            SceneContent.SetRole(place, character);
        }
        
        // 交互方法
        public void SetInteraction(RolePlacing.Index place, int selection)
        {
            var rolePlacing = GetRolePlacing(place);
            rolePlacing.SetInteraction(selection);
            if (rolePlacing.Interaction == RolePlacing.Interactions.Orientation)
                SceneContent.SetOrientation(place, (RolePlacing.Facing)selection);
        }
    }
}