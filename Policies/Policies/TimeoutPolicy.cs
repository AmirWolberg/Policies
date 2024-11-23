using System;
using System.Diagnostics;

namespace Policies.Policies
{
    /// <summary>
    /// Interface representing a timer used in the <see cref="TimeoutPolicy"/> class
    /// </summary>
    public interface ITimer
    {
        void Restart();
        TimeSpan Elapsed { get; }
    }

    internal class Timer : ITimer
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public void Restart() => _stopwatch.Restart();
        public TimeSpan Elapsed => _stopwatch.Elapsed;
    }
    
    /// <summary>
    /// A policy for looping for a given amount of time
    /// </summary>
    public class TimeoutPolicy: BasePolicy
    {
        private readonly ITimer _timer;
        private readonly TimeSpan _timeout;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeoutPolicy"/> policy
        /// </summary>
        /// <param name="timeout"> The total amount of time to loop when applying this policy </param>
        /// <param name="timer"> Timer used to count the elapsed time used to determine when to end the policy </param>
        public TimeoutPolicy(TimeSpan timeout, ITimer? timer = null)
        {
            _timeout = timeout;
            _timer = timer ?? new Timer();
        }
        
        protected override void Initialize()
        {
            _timer.Restart();
        }
        
        protected override bool Completed() => _timeout <= _timer.Elapsed;

        protected override bool Completed<TOut>(TOut output) => Completed();
    }
}