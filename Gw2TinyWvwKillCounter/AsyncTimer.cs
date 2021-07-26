using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gw2TinyWvwKillCounter
{
    // This is my attempt to create an async await based timer instead of using one of the .NET timer classes. 
    // exceptions or starting multiple timers in parallel due to reentrancy and race conditions can easily happen around the awaits in this class. 
    // workaround if this causes too much problems: use one of .NET Timer class. start it once, never stop it. handle different cases in the eventHandler/callback of the timer.
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
            await Stop();

            _cancellationTokenSource = new CancellationTokenSource();
            _isRunning = true;

            while (_isRunning)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(_intervalInSeconds), _cancellationTokenSource.Token);
                    IntervalEnded?.Invoke(this, EventArgs.Empty);
                }
                catch (OperationCanceledException)
                {
                    _isRunning = false;
                }
            }
        }

        public async Task Stop()
        {
            if (_isRunning)
            {
                _cancellationTokenSource?.Cancel();

                while (_isRunning)
                    await Task.Delay(500);
            }
        }

        public event EventHandler IntervalEnded;

        private CancellationTokenSource _cancellationTokenSource;
        private bool _isRunning;
        private readonly int _intervalInSeconds;
    }
}