using UnityEngine.Events;
using XNode;
using System;

public abstract class XNodeBase : Node
{
    protected void UpdatePortConnection(string fieldName, Action<NodePort> hasConnectionAction, Action<NodePort> noConnectionAction)
    {
        var ownPort = GetPort(fieldName);
        if (ownPort != null)
        {
            if (!ownPort.fieldName.StartsWith(fieldName)) return;
            if (!ownPort.IsConnected)
            {
                noConnectionAction(ownPort);
                return;
            }
        }

        hasConnectionAction(ownPort);
    }
    
    protected void UpdatePortListConnection(string fieldName,int arrayCount , Action<NodePort,int> hasConnectionAction, Action<NodePort,int> noConnectionAction)
    {
        for (var i = 0; i < arrayCount; i++)
        {
            var index = i;
            UpdatePortConnection($"{fieldName} {i}", p => hasConnectionAction(p, index),
                p => noConnectionAction(p, index));
        }
    }

    #region Connection
    public override void OnCreateConnection(NodePort from, NodePort to)
    {
        ConnectionNodeUpdate();
        base.OnCreateConnection(from, to);
    }
    public override void OnRemoveConnection(NodePort port)
    {
        ConnectionNodeUpdate();
        base.OnRemoveConnection(port);
    }
    #endregion
    protected void OnPortConnected(NodePort ownPort, string fieldName, UnityAction action)
    {
        if (ownPort.node == this && ownPort.fieldName.StartsWith(fieldName)) action();
    }

    protected void OnPortDisconnected(string fieldName, UnityAction action)
    {
        var port = GetPort(fieldName);
        if (!port.IsConnected) action();
    }

    protected abstract void ConnectionNodeUpdate();
}