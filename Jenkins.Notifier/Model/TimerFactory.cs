namespace Jenkins.Notifier.Model
{
    public class TimerFactory : ITimerFactory
    {
        public ITimer Create(int refreshDelay)
        {
            return new Timer(refreshDelay);
        }
    }
}
