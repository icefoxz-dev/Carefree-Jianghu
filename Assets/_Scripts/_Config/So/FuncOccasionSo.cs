using System;
using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "FuncOccasionSo", menuName = "配置/场合/功能")]
    public class FuncOccasionSo : OccasionSo
    {
        private readonly FuncTagSo[] _results;

        public override IFuncTag[] Results => _results;
    }
}