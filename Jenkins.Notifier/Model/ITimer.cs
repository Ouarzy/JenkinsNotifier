namespace Jenkins.Notifier.Model
{
    using System;

    public interface ITimer : IDisposable
    {
        event TimerEventHandler Tick;
    }
}
