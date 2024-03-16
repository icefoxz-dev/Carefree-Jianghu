namespace _Game._Controllers
{
    public class ChallengeController : ControllerBase
    {
        public void Instance() => Game.World.InstanceChallenge();
        public void Start() => Game.World.StartChallenge();
        public void Finalize() => Game.World.FinalizeChallenge();
    }
}