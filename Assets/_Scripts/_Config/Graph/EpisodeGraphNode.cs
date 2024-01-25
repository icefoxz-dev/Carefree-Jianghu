using _Config.So;
using System.Linq;
using _Data;
using UnityEngine.Events;
using XNode;

public abstract class XNodeBase : Node
{
    protected void OnPortConnected(NodePort ownPort, string fieldName, UnityAction action)
    {
        if (ownPort.node == this && ownPort.fieldName.StartsWith(fieldName)) action();
    }

    protected void OnPortDisconnected(string fieldName, UnityAction action)
    {
        var port = GetPort(fieldName);
        if (!port.IsConnected) action();
    }
}

public abstract class EpisodeGraphNode : XNodeBase, IEpisodeGraphNode
{
    public abstract string NodeName { get; }
}
public abstract class EpisodeNodeBase : EpisodeGraphNode
{
    public abstract void SetPrevNode(int index, EpisodeNodeBase node);
    public abstract void SetNextNode(int index, EpisodeNodeBase node);

    /// <summary>
    /// 根据NodePort的名字获取索引, 规范是: "[fieldName][space][index]"
    /// </summary>
    /// <param name="port"></param>
    /// <returns></returns>
    protected int GetNameIndex(NodePort port) =>
        int.TryParse(port.fieldName.Split(' ').Last(), out var index) ? index : -1;
}

public abstract class EpisodeNode : EpisodeNodeBase, IEpisodeNode
{
    public abstract Occasion.Phase Phase { get; }
    public abstract IEpisodeNode[] GetNextNodes();
    IOccasion IEpisodeNode.Occasion => Occasion?.GetOccasion();
    public virtual int GetEpNodeOrder(IRoleTerm role) => 0;
    public override string NodeName
    {
        get
        {
            var name = Occasion?.So?.name;
            return string.IsNullOrWhiteSpace(name) ?  "Undefined" :  name;
        }
    }
    [Input(ShowBackingValue.Always,ConnectionType.Override)]
    public OccasionNode Occasion;

    public override void OnCreateConnection(NodePort from, NodePort to)
    {
        OnPortConnected(to, nameof(Occasion), () => Occasion = from.node as OccasionNode);
        base.OnCreateConnection(from, to);
    }

    public override void OnRemoveConnection(NodePort port)
    {
        OnPortDisconnected(nameof(Occasion), () => Occasion = null);
        base.OnRemoveConnection(port);
    }
}
