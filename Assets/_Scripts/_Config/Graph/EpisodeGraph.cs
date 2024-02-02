using System;
using System.Collections.Generic;
using System.Linq;
using _Config;
using _Data;
using Sirenix.OdinInspector;
using UnityEngine;
using XNode;

public interface IEpisodeGraphNode
{
    string NodeName { get; }
}

[CreateAssetMenu(fileName = "EpisodeGraph", menuName = "配置/故事/剧集蓝图")]
public class EpisodeGraph : AutoNameGraphBase, IDataElement
{
    [SerializeField] private int id;
    [TextArea] public string Brief;
    public int Id => id;


    protected override string Prefix => Id.ToString();
    private const char CSeparator = '#';
    protected override char Separator => CSeparator;

    [OnInspectorGUI(nameof(SetElementName))]
    private void SetElementName()
    {
        foreach (var node in nodes.Cast<Node>())
        {
            if (node is IEpisodeGraphNode episodeNode)
                node.name = episodeNode.NodeName;
        }
    }

    public EpisodeData GetEpisode()
    {
        const int RecrusiveLimit = 1000;
        var list = new List<IOccasion>();
        foreach (var nodeBase in nodes.Cast<XNodeBase>())
        {
            if (nodeBase is IOccasion o)
                list.Add(o);
        }

        var root = list.FirstOrDefault(n => n.EpNode.Phase == Occasion.Phase.Root);
        if (root == null) Debug.LogError($"{name} 没有根节点!", this);
        var recursive = 0;
        var map = new Dictionary<int, List<IEpisodeNode>>();
        var index = 0;
        map.Add(0, new List<IEpisodeNode> { root.EpNode });
        index++;
        ResolveNextNodes(root.EpNode.GetNextNodes(), map, index,ref recursive);

        return new EpisodeData(Id, Brief, list.ToArray(), map);

        void ResolveNextNodes(IEpisodeNode[] node, Dictionary<int, List<IEpisodeNode>> map, int index, ref int recursive)
        {
            foreach (var nextNode in node)
            {
                recursive++;
                if (recursive > RecrusiveLimit)
                    throw new Exception($"递归次数超过{RecrusiveLimit}次,请检查{nextNode.Occasion.Name}的连接是否有问题!");
                if (!map.ContainsKey(index)) map.Add(index, new List<IEpisodeNode>());
                map[index].Add(nextNode);
                ResolveNextNodes(nextNode.GetNextNodes(), map, index + 1, ref recursive);
            }
        }
    }


}