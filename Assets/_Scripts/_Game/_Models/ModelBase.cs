using UniMvc.Utls;

namespace _Game._Models
{
    public abstract class ModelBase
    {
        protected void SendEvent(string eventName, params object[] args) => Game.MessagingManager.Send(eventName, args);
    }
}