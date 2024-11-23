using System;
using System.Collections.Generic;

namespace Policies
{
    /// <summary>
    /// Serves as the foundation for defining policies that can be applied to recurring actions.
    /// It implements the <see cref="IPolicy"/> interface and adds support for the creation of a chain of policies
    /// through its <see cref="Extend"/> method
    /// </summary>
    public abstract class BasePolicy : IPolicy
    {
        private BasePolicy? _chainedPolicy;

        /// <summary>
        /// Adds a policy to the current policy chain
        /// </summary>
        /// <param name="chainedPolicy">The <see cref="BasePolicy"/> to add to the policy chain</param>
        /// <returns>The policy chain created by the function</returns>
        public BasePolicy? Extend(BasePolicy? chainedPolicy)
        {
            if (_chainedPolicy != null) return _chainedPolicy.Extend(chainedPolicy);
            _chainedPolicy = chainedPolicy;
            return this;
        }

        /// <summary>
        /// Initializes policy before starting the policy's loop.
        /// </summary>
        protected virtual void Initialize() { }
        
        /// <summary>
        /// Invokes <see cref="Initialize"/> in <see cref="_chainedPolicy"/> and in current policy.
        /// </summary>
        protected void ChainedInitialize()
        {
            _chainedPolicy?.Initialize();
            Initialize();
        }

        /// <summary>
        /// Places Policy on hold every time it loops until true is returned.
        /// </summary>
        /// <param name="item"> Current item being iterated over by the policy in the policy loop </param>
        /// <returns> Whether the policy should stop waiting and continue performing
        /// another loop iteration (true) or keep waiting (false) </returns>
        protected virtual bool ShouldApply<TItem>(TItem item) => true;

        /// <summary>
        /// Invokes <see cref="ShouldApply"/> in <see cref="_chainedPolicy"/> and in current policy and if either is
        /// true continues loop iteration, if both are false keeps waiting.
        /// </summary>
        protected bool ChainedShouldApply<TItem>(TItem item) => ShouldApply(item) || (_chainedPolicy?.ShouldApply(item) ?? false);
        
        /// <summary>
        /// Places Policy on hold every time it loops until true is returned.
        /// </summary>
        /// <returns> Whether the policy should stop waiting and continue performing
        /// another loop iteration (true) or keep waiting (false) </returns>
        protected virtual bool ShouldApply() => true;
        
        /// <summary>
        /// Invokes <see cref="ShouldApply"/> in <see cref="_chainedPolicy"/> and in current policy and if either is
        /// true continues loop iteration, if both are false keeps waiting.
        /// </summary>
        protected bool ChainedShouldApply() => ShouldApply() || (_chainedPolicy?.ShouldApply() ?? false);

        /// <summary>
        /// Checks whether to terminate the policy loop every time it loops.
        /// </summary>
        /// <param name="output"> The output of the func from the current policy loop iteration </param>
        /// <returns> Whether the policy has completed (true) or not (false) </returns>
        protected virtual bool Completed<TOut>(TOut output) => false;

        /// <summary>
        /// Invokes <see cref="Completed"/> in <see cref="_chainedPolicy"/> and in current policy and if either is
        /// true continues terminates the policy loop, if both are false keeps looping.
        /// </summary>
        protected bool ChainedCompleted<TOut>(TOut output) => Completed(output) || (_chainedPolicy?.Completed(output) ?? false);
        
        /// <summary>
        /// Checks whether to terminate the policy loop every time it loops.
        /// </summary>
        /// <returns> Whether the policy has completed (true) or not (false) </returns>
        protected virtual bool Completed() => false;
        
        /// <summary>
        /// Invokes <see cref="Completed"/> in <see cref="_chainedPolicy"/> and in current policy and if either is
        /// true continues terminates the policy loop, if both are false keeps looping.
        /// </summary>
        protected bool ChainedCompleted() => Completed() || (_chainedPolicy?.Completed() ?? false);
        
        /// <summary>
        /// Performs a mutation in the policy every time it loops.
        /// </summary>
        protected virtual void Mutate() { }
        
        /// <summary>
        /// Invokes <see cref="Mutate"/> in <see cref="_chainedPolicy"/> and in current policy.
        /// </summary>
        protected void ChainedMutate()
        {
            _chainedPolicy?.Mutate();
            Mutate();
        }

        /// <inheritdoc />
        public IEnumerable<TOut> Apply<TItem, TOut>(IEnumerable<TItem> items, Func<TItem, TOut> func)
        {
            ChainedInitialize();
            foreach (var item in items)
            {
                if (ChainedCompleted()) break;
                while(!ChainedShouldApply(item)){}
                var output = func(item);
                ChainedMutate();
                yield return output;
                if (ChainedCompleted(output)) break;
            }
        }

        /// <inheritdoc />
        public void Apply<TItem>(IEnumerable<TItem> items, Action<TItem> action)
        {
            ChainedInitialize();
            foreach (var item in items)
            {
                if (ChainedCompleted()) break;
                while(!ChainedShouldApply(item)){}
                action(item);
                ChainedMutate();
                if (ChainedCompleted()) break;
            }
        }

        /// <inheritdoc />
        public IEnumerable<TOut> Apply<TOut>(Func<TOut> func)
        {
            ChainedInitialize();
            while (!ChainedCompleted())
            {
                while(!ChainedShouldApply()){}
                var output = func();
                ChainedMutate();
                yield return output;
                if (ChainedCompleted(output)) break;
            }
        }

        /// <inheritdoc />
        public void Apply(Action action)
        {
            ChainedInitialize();
            while (!ChainedCompleted())
            {
                while(!ChainedShouldApply()){}
                action();
                ChainedMutate();
            }
        }
    }
}