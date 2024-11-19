namespace Policies.Policies
{
    /// <summary>
    /// A policy for looping a given amount of times
    /// </summary>
    public class CountPolicy: BasePolicy
    {
        private ulong _count;
        private readonly ulong _amount;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CountPolicy"/> policy
        /// </summary>
        /// <param name="amount"> The amount of times to loop when applying this policy </param>
        public CountPolicy(ulong amount)
        {
            _amount = amount;
        }
        
        protected override void Initialize()
        {
            base.Initialize();
            _count = 0;
        }
        
        protected override void Mutate()
        {
            _count++;
            base.Mutate();
        }

        protected override bool Completed() => _amount <= _count || base.Completed();

        protected override bool Completed<TOut>(TOut output) => _amount <= _count || base.Completed(output);
    }
}