//using UnityEngine;
//using XNode;

//public abstract class TransitNode : OccasionNode
//{


//    public override void OnCreateConnection(NodePort from, NodePort to)
//    {
//        _prev = from.node as SceneNode;
//        base.OnCreateConnection(from, to);
//    }

//    public override void OnRemoveConnection(NodePort port)
//    {
//        _prev = null;
//        base.OnRemoveConnection(port);
//    }
//}