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
        private BasePolicy? _basePolicy;

        /// <summary>
        /// Adds a policy to the current policy chain
        /// </summary>
        /// <param name="basePolicy">The <see cref="BasePolicy"/> to add to the policy chain</param>
        /// <returns>The policy chain created by the function</returns>
        public BasePolicy? Extend(BasePolicy? basePolicy)
        {
            if (_basePolicy != null) return _basePolicy.Extend(basePolicy);
            _basePolicy = basePolicy;
            return this;
        }

        /// <summary>
        /// Initializes policy before starting the policy's loop.
        /// Triggers function in chained policies 
        /// </summary>
        protected virtual void Initialize()
        {
            _basePolicy?.Initialize();
        }

        /// <summary>
        /// Places Policy on hold every time it loops until true is returned.
        /// Uses function in chained policies to determine result
        /// </summary>
        /// <param name="item"> Current item being iterated over by the policy in the policy loop </param>
        /// <returns> Whether the policy should stop waiting and continue performing
        /// another loop iteration (true) or keep waiting (false) </returns>
        protected virtual bool ShouldApply<TItem>(TItem item) => _basePolicy == null || _basePolicy.ShouldApply(item);

        /// <summary>
        /// Places Policy on hold every time it loops until true is returned.
        /// Uses function in chained policies to determine result
        /// </summary>
        /// <returns> Whether the policy should stop waiting and continue performing
        /// another loop iteration (true) or keep waiting (false) </returns>
        protected virtual bool ShouldApply() => _basePolicy == null || _basePolicy.ShouldApply();

        /// <summary>
        /// Checks whether to terminate the policy loop every time it loops.
        /// Uses function in chained policies to determine result
        /// </summary>
        /// <param name="output"> The output of the func from the current policy loop iteration </param>
        /// <returns> Whether the policy has completed (true) or not (false) </returns>
        protected virtual bool Completed<TOut>(TOut output) => _basePolicy != null && _basePolicy.Completed(output);

        /// <summary>
        /// Checks whether to terminate the policy loop every time it loops.
        /// Uses function in chained policies to determine result
        /// </summary>
        /// <returns> Whether the policy has completed (true) or not (false) </returns>
        protected virtual bool Completed() => _basePolicy != null && _basePolicy.Completed();

        /// <summary>
        /// Performs a mutation in the policy every time it loops.
        /// Triggers function in chained policies
        /// </summary>
        protected virtual void Mutate()
        {
            _basePolicy?.Mutate();
        }

        /// <inheritdoc />
        public IEnumerable<TOut> Apply<TItem, TOut>(IEnumerable<TItem> items, Func<TItem, TOut> func)
        {
            Initialize();
            foreach (var item in items)
            {
                if (Completed()) break;
                while(!ShouldApply(item)){}
                var output = func(item);
                Mutate();
                yield return output;
                if (Completed(output)) break;
            }
        }

        /// <inheritdoc />
        public void Apply<TItem>(IEnumerable<TItem> items, Action<TItem> action)
        {
            Initialize();
            foreach (var item in items)
            {
                if (Completed()) break;
                while(!ShouldApply(item)){}
                action(item);
                Mutate();
                if (Completed()) break;
            }
        }

        /// <inheritdoc />
        public IEnumerable<TOut> Apply<TOut>(Func<TOut> func)
        {
            Initialize();
            while (!Completed())
            {
                while(!ShouldApply()){}
                var output = func();
                if (output == null) break; // Func returning null is a sign to break iteration - todo 
                Mutate();
                yield return output;
                if (Completed(output)) break;
            }
        }

        /// <inheritdoc />
        public void Apply(Action action)
        {
            Initialize();
            while (!Completed())
            {
                while(!ShouldApply()){}
                action();
                Mutate();
            }
        }
    }
}