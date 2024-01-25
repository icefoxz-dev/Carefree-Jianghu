using System;
using _Config.So;
using Sirenix.OdinInspector;
using UniMvc.Core;
using UniMvc.Utls;
using UnityEngine;

namespace _Game
{
    public class AppLaunch : MonoBehaviour
    {
        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private UiManagerBase _uiManager;
        [SerializeField] private Res _res;
        [SerializeField] private MonoService _monoService;
        [SerializeField] private GestureHandler _gestureHandler;
        [SerializeField] private ConfigSo _config;

        void Start()
        {
            Game.Run(_res, _monoService, _mainCanvas, _uiManager, _config);
        }

#if UNITY_EDITOR
        [SerializeField] private IAspectRatioUpdater[] _updaters;
        [Button]private void UpdateAspectRatio()
        {
            Array.ForEach(_updaters, u => u.UpdateAspectRatio());
        }
#endif
    }
}