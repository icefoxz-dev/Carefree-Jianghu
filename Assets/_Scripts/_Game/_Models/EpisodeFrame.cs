using _Data;

namespace _Game._Models
{
    public class EpisodeFrame : IOccasion
    {
        private RolePlay[] Roles { get; }
        public Occasion.PlaceMode PlaceMode { get; }

        public IEpisodeNode EpNode => throw new System.NotImplementedException();

        public string Name { get; }
        public string Description { get; }

        public ISceneContent SceneContent { get; }

        IRolePlay[] IOccasion.Roles => Roles;

        public EpisodeFrame(IOccasion o)
        {
            PlaceMode = o.PlaceMode;
            Name = o.Name;
            Description = o.Description;
            SceneContent = o.SceneContent;
            Roles = PlaceMode == Occasion.PlaceMode.Solo ? new RolePlay[1] : new RolePlay[2];
        }

        public void SetRole(RolePlay rolePlay, int placeIndex)
        {
            Roles[placeIndex] = rolePlay;
        }
    }
}