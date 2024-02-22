using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "FuncTagSo", menuName = "配置/标签/功能")]
    public class FuncTagSo : AutoNameSoBase,IFuncTag
    {
        [SerializeField] private GameTagSoBase _gameTag;
        [SerializeField] private double _value = 1;
        public IGameTag GameTag => _gameTag;
        public double Value => _value;

        public void SetPlayer(IRoleData player)
        {
            var manager = _gameTag.GetTagManager(player.Prop);
            manager.AddTagValue(this);
        }
    }
}