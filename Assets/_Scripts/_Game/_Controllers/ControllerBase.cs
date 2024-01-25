using _Game._Models;
using UniMvc.Core;

namespace _Game._Controllers
{
    /// <summary>
    /// 控制器, 自动注册Game
    /// </summary>
    public abstract class ControllerBase : IController
    {
        protected GameWorld World { get; private set; }
        
        public ControllerBase Reg(GameWorld world)
        {
            World = world;
            return this;
        }
    }
}