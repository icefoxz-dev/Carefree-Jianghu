using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using XNode;

public abstract class AutoNameGraphBase : NodeGraph
{
    [ReadOnly][SerializeField] private ScriptableObject _so;
#if UNITY_EDITOR
    [OnInspectorGUI(nameof(GetReference))]
    protected void GetReference()
    {
        if (_so == null) _so = this;
        ChangeName();
        return;

        void ChangeName()
        {
            var path = AssetDatabase.GetAssetPath(this);
            var newName = GetName();
            if (string.IsNullOrWhiteSpace(newName))
                return;
            var err = AssetDatabase.RenameAsset(path, newName);
            if (!string.IsNullOrWhiteSpace(err)) Debug.LogError(err);
        }
    }
#endif

    protected virtual string Prefix { get; }
    protected virtual string Suffix { get; }
    protected abstract char Separator { get; }

    public virtual string Name => _name;
    [SerializeField] protected string _name;

    private string GetName() => (Prefix ?? "") + Separator + _name + (Suffix ?? "");
}

public abstract class AutoNameNodeBase : Node
{
    [ReadOnly][SerializeField] private ScriptableObject _so;
#if UNITY_EDITOR
    [OnInspectorGUI(nameof(GetReference))]
    protected void GetReference()
    {
        if (_so == null) _so = this;
        ChangeName();
        return;

        void ChangeName()
        {
            var path = AssetDatabase.GetAssetPath(this);
            var newName = GetName();
            if (string.IsNullOrWhiteSpace(newName))
                return;
            var err = AssetDatabase.RenameAsset(path, newName);
            if (!string.IsNullOrWhiteSpace(err)) Debug.LogError(err);
        }
    }
#endif
    protected virtual string Prefix { get; }
    protected virtual string Suffix { get; }
    protected abstract char Separator { get; }

    public virtual string Name => _name;
    [SerializeField] protected string _name;

    private string GetName() => (Prefix ?? "") + Separator + _name + (Suffix ?? "");
}