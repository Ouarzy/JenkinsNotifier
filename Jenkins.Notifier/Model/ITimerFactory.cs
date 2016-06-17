namespace Jenkins.Notifier.Model
{
    public interface ITimerFactory
    {
        ITimer Create(int refreshDelay);
    }
}