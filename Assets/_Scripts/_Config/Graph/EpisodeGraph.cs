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
public class EpisodeGraph : AutoNameGraphBase,IDataElement
{
    [SerializeField]private int id;
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
        var list = new List<OccasionData>();
        return new EpisodeData(Id, Brief, list.ToArray());
    }
}