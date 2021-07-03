using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gw2TinyWvwKillCounter
{
    public class AsyncTimer
    {
        public async void Start(uint intervalInSeconds)
        {
            StopTimerIfRunning();
            _timerIsRunning = true;
            _timerCancellationTokenSource = new CancellationTokenSource();

            while (_timerIsRunning)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(intervalInSeconds), _timerCancellationTokenSource.Token);
                    IntervalEnded?.Invoke(this, EventArgs.Empty);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        public void Stop()
        {
            StopTimerIfRunning();
        }

        private void StopTimerIfRunning()
        {
            if (_timerIsRunning)
            {
                _timerCancellationTokenSource?.Cancel();
                _timerIsRunning = false;
            }
        }

        public event EventHandler IntervalEnded;
        private CancellationTokenSource _timerCancellationTokenSource;
        private bool _timerIsRunning;
    }
}