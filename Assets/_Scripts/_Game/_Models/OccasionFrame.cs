using System;
using System.Collections.Generic;
using System.Linq;
using _Data;

namespace _Game._Models
{
    public class OccasionModel : ModelBase, IOccasion
    {
        private readonly IOccasion _ref;
        public Occasion.Modes Mode => Occasion.Modes.Solo;
        public string Name { get; }
        public string Description { get; }

        public RolePlacing[] PlacingList { get; }

        public OccasionModel(IOccasion o)
        {
            Name = o.Name;
            PlacingList = o.GetPlacingInfos().Select(p => new RolePlacing(p)).ToArray();
            Description = o.Description;
            _ref = o;
        }

        public IRolePlacing[] GetPlacingInfos() => PlacingList.ToArray();

        public string GetLine(RolePlacing.Index role, int index) => $"测试, role = {role}, index = {index}";

        public void UpdateRole(IRoleData role)
        {
            _ref.UpdateRole(role);
            SendEvent(GameEvent.Role_Update, role.Character.Id);
        }

        public IPlotTerm[] GetExcludedTerms(IRoleData role) => _ref.GetExcludedTerms(role);

        public void SetRole(RolePlacing.Index placeIndex, ICharacter role)
        {
            var placing = PlacingList.FirstOrDefault(p => p.Place == placeIndex);
            if (placing==null)
            {
                throw new NullReferenceException($"PlaceIndex {placeIndex} not found");
            }
            placing?.PlaceCharacter(role);
            SendEvent(GameEvent.Occasion_Update);
        }
    }

    public class OccasionFrame : ModelBase, IOccasion
    {
        /**
         * 1. 场景信息，描述，演示，预制体，动画
         * 2. 场景模式, solo, versus
         * 3. 交互模式, 无, 选项, 方向
         * 4. 角色信息，对话内容
         */

        #region IOccasion
        private readonly IOccasion _ref;
        public Occasion.Modes Mode => _ref.Mode;
        public string Name => _ref.Name;
        public string Description => _ref.Description;

        public IRolePlacing[] GetPlacingInfos() => _ref.GetPlacingInfos();

        public string GetLine(RolePlacing.Index role, int index) => _ref.GetLine(role, index);
        public void UpdateRole(IRoleData role) => _ref.UpdateRole(role);
        public IPlotTerm[] GetExcludedTerms(IRoleData role) => _ref.GetExcludedTerms(role);

        public void SetRole(RolePlacing.Index placeIndex, ICharacter role)
        {
            var c = _characters.FirstOrDefault(c => c.Place == placeIndex);
            c?.PlaceCharacter(role);
        }

        #endregion

        private readonly List<RolePlacing> _characters;
        public IReadOnlyList<RolePlacing> Characters => _characters;
        //public ISceneContent SceneContent => SceneFrame.CurrentScene;
        //public SceneFrame SceneFrame { get; }
        public int FrameIndex { get; }

        public OccasionFrame(IOccasion o, int frameIndex)
        {
            _ref = o;
            FrameIndex = frameIndex;
            //SceneFrame = Game.Scene.GetFrame(frameIndex);
            //SceneFrame.SetSceneContent(o.SceneContent);
            _characters = o.GetPlacingInfos().Select(p => new RolePlacing(p)).ToList();
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
            //SceneContent.SetRole(place, character);
        }
    }

}