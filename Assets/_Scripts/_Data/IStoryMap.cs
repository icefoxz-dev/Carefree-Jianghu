namespace _Data
{
    //故事映射，用于定义故事剧情章节
    public interface IStoryMap
    {
        IStoryActivities Activities { get; }
        string[] Intro { get; }
    }
    //故事活动，用于定义故事的发展和剧情
    public interface IStoryActivities
    {
        IPurpose[] GetMandatoryPurposes(IRoleData role, IGameRound round);
        IOccasionCluster[] GetClusters();
    }
}