namespace _Data
{
    /// <summary>
    /// 标签值, 根据标签类型提供值用于判断<see cref="ITagTerm"/>和赋值
    /// </summary>
    public interface ITagValue 
    {
        IGameTag Tag { get; }
        double Value { get; }
    }
}