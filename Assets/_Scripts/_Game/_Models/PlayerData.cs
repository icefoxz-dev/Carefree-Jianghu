using _Data;

namespace _Game._Models
{
    /// <summary>
    /// 玩家信息，包括玩家的角色信息和玩家的游戏数据。
    /// </summary>
    public class PlayerData : IPlayerData,IPlayerProperty
    {
        private readonly Character _character;
        private TagManager _trait = new TagManager();
        private readonly TagManager _capable = new TagManager();
        private readonly TagManager _skill = new TagManager();
        private readonly TagManager _episodeTag = new TagManager();
        private readonly TagManager _chapterTag = new TagManager();
        private readonly TagManager funcTag = new TagManager();
        public ITagManager Trait => _trait;
        public ITagManager Capable => _capable;
        public ITagManager Skill => _skill;
        public ITagManager EpisodeTag => _episodeTag;
        public ITagManager ChapterTag => _chapterTag;
        public ITagManager FuncTag => funcTag;

        public IPlayerProperty Prop => this;
        public ICharacter Character => _character;
    }
}