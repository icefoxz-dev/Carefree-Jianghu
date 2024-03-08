using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace _Config
{
    /// <summary>
    /// 自动文件名SO
    /// </summary>
    public class AutoUnderscoreNamingObject : AutoNameWithSeparatorSoBase
    {
        private const char Underscore = '_';
        protected override char Separator => Underscore;
    }
    public class AutoDashNamingObject : AutoNameWithSeparatorSoBase
    {
        private const char Dash = '-';
        protected override char Separator => Dash;
    }
    public class AutoBacktickNamingObject : AutoNameWithSeparatorSoBase
    {
        private const char Backtick = '`';
        protected override char Separator => Backtick;
    }
    public class AutoHashNamingObject : AutoNameWithSeparatorSoBase
    {
        private const char Hash = '#';
        protected override char Separator => Hash;
    }
    public class AutoAtNamingObject : AutoNameWithSeparatorSoBase
    {
        private const char At = '@';
        protected override char Separator => At;
    }

    public abstract class AutoNameWithSeparatorSoBase : AutoNameSoBase, IDataElement
    {
        public virtual int Id => id;
        [SerializeField] protected int id;

        protected abstract char Separator { get; }
        protected override string GetName() => string.Join(Separator, id, base.GetName());
    }

    public abstract class AutoNameSoBase : ScriptableObject
    {
        [ReadOnly][SerializeField] private ScriptableObject referenceSo;
#if UNITY_EDITOR
        [OnInspectorGUI(nameof(GetReference))]
        protected void GetReference()
        {
            if (referenceSo == null) referenceSo = this;
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

        public virtual string Name => _name;
        [SerializeField] protected string _name;

        protected virtual string GetName() => (Prefix ?? "") + _name + (Suffix ?? "");
    }
}