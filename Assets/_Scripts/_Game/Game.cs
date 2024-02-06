using System;
using _Config.So;
using _Game._Controllers;
using _Game._Models;
using UniMvc.Core;
using UniMvc.Utls;
using UnityEngine;

namespace _Game
{
    public static class Game
    {
        public static Canvas MainCanvas { get; private set; }
        public static Camera MainCamera => MainCanvas.worldCamera;
        private static MonoService _monoService;
        private static GameWorld _world = new GameWorld();
        private static GameScene _scene;
        private static bool IsRunning { get; set; }
        private static ControllerServiceContainer ServiceContainer { get; set; }

        public static T GetController<T>() where T : ControllerBase, new()
        {
            var type = typeof(T);
            if (ServiceContainer.TryGet<T>(type, out var ctr)) return ctr;
            //第一次会自动创建. 但依然由ServiceContainer给出
            RegController();
            return ServiceContainer.Get<T>();

            void RegController()
            {
                var c = new T();
                c.Reg(_world);
                ServiceContainer.Reg(c);
            }
        }

        public static Res Res { get; private set; }
        public static UiBuilder UiBuilder { get; private set; }
        public static MessagingManager MessagingManager { get; } = new MessagingManager();
        public static IMainThreadDispatcher MainThread { get; private set; }
        public static ConfigSo Config { get; private set; }

        public static IGameWorld World => _world;

        public static MonoService MonoService
        {
            get
            {
                if (_monoService == null)
                    _monoService = new GameObject("MonoService").AddComponent<MonoService>();
                return _monoService;
            }
            private set => _monoService = value;
        }
        public static IGameScene Scene => _scene;

        public static void Run(Res res, MonoService monoService, Canvas mainCanvas, UiManagerBase uiManager,
            ConfigSo config, GameScene scene)
        {
            if (IsRunning)
                throw new NotImplementedException("App is running!");
            IsRunning = true;
            Config = config;
            _monoService = monoService;
            _scene = scene;
            MainCanvas = mainCanvas;
            Res = res;
            MainThread = MonoService.gameObject.AddComponent<MainThreadDispatcher>();
            ServiceContainer = new ControllerServiceContainer();
            RegEvents();
            uiManager.Init();
            scene.Init();
            return;

            void RegEvents()
            {

            }
        }

        public static void SendEvent(string eventName, DataBag bag) => MessagingManager.Send(eventName, bag);
        public static void SendEvent(string eventName, params object[] args)
        {
            args ??= Array.Empty<object>();
            MessagingManager.Send(eventName, args);
        }

        public static void RegEvent(string eventName, Action<DataBag> callbackAction) => MessagingManager.RegEvent(eventName, callbackAction);

        public static bool IsInRect(RectTransform rect,Vector2 screenPoint,Camera camera) => RectTransformUtility.RectangleContainsScreenPoint(rect, screenPoint, camera);
    }
}
