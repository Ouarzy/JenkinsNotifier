namespace Jenkins.Notifier.Tests.ViewModel
{
    using Jenkins.Notifier.Model;

    public class TimerFake : ITimer
    {
        public event TimerEventHandler Tick;

        public void RaiseTick()
        {
            this.OnTick();
        }
        
        protected virtual void OnTick()
        {
            var handler = this.Tick;
            if (handler != null)
            {
                handler(this, new TimerEventArgs());
            }
        }

        public void Dispose()
        {
        }
    }
}