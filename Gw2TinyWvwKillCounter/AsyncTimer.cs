using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gw2TinyWvwKillCounter
{
    public class AsyncTimer
    {
        public AsyncTimer(int intervalInSeconds)
        {
            _intervalInSeconds = intervalInSeconds;
        }

        public async void Start()
        {
            // stop() call could be replaced by a if(_timerIsRunning)-guard, but then next interval after Start() ends "randomly"
            // in less than _intervalInSeconds because the timer is already running.
            Stop();
            _isRunning               = true;
            _cancellationTokenSource = new CancellationTokenSource();

            while (_isRunning)
            {
                await Task.Delay(TimeSpan.FromSeconds(_intervalInSeconds), _cancellationTokenSource.Token);

                if (_isRunning) // not sure if necessary: Stop() called between Task.Delay ended and continuation on message pump is called? 
                    IntervalEnded?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Stop()
        {
            if (_isRunning)
            {
                _cancellationTokenSource?.Cancel();
                _isRunning = false;
            }
        }

        public event EventHandler IntervalEnded;

        private CancellationTokenSource _cancellationTokenSource;
        private bool _isRunning;
        private readonly int _intervalInSeconds;
    }
}