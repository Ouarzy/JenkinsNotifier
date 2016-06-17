namespace Jenkins.Notifier.Model
{
    using System.Timers;

    public sealed class Timer : ITimer
    {
        private readonly System.Timers.Timer timer;

        public event TimerEventHandler Tick;

        public Timer(int refreshDelay)
        {
            this.timer = new System.Timers.Timer(refreshDelay * 1000) { AutoReset = true };
            this.timer.Elapsed += this.OnTimerTick;
            this.timer.Start();
        }

        private void OnTimerTick(object state, ElapsedEventArgs elapsedEventArgs)
        {
            this.OnTick();
        }

        private void OnTick()
        {
            var handler = this.Tick;
            if (handler != null)
            {
                handler(this, new TimerEventArgs());
            }
        }

        public void Dispose()
        {
            this.timer.Stop();
            this.timer.Close();
            this.timer.Dispose();
        }
    }
}
