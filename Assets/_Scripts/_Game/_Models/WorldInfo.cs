namespace _Game._Models
{
    public class WorldInfo : ModelBase
    {
        public int Round { get; private set; }

        public void NextRound()
        {
            Round++;
            SendEvent(GameEvent.Round_Update);
        } 
    }
}