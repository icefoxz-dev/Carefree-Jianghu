using UniMvc.Core;

namespace _Game._Controllers
{
    public class MainController : ControllerBase
    {
        public void StartGame() => World.Init();
    }
}
