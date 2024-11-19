using System;
using System.Collections.Generic;

namespace Policies
{
    /// <summary>
    /// Interface representing a policy.
    /// A policy is a rule that can be applied to a recurring action to alter the way it recurs according
    /// to the policy definition.
    /// todo make the definition of poilicy more clear and accurate
    /// </summary>
    public interface IPolicy
    {
        /// <summary>
        /// Applies policy on given enumerable with given func and returns the results
        /// </summary>
        IEnumerable<TOut> Apply<TItem, TOut>(IEnumerable<TItem> items, Func<TItem, TOut> func);
        
        /// <summary>
        /// Applies policy on given enumerable with given action
        /// </summary>
        void Apply<TItem>(IEnumerable<TItem> items, Action<TItem> action);
        
        /// <summary>
        /// Applies policy on given func and returns the results
        /// </summary>
        IEnumerable<TOut> Apply<TOut>(Func<TOut> func);

        /// <summary>
        /// Applies policy on given action
        /// </summary>
        /// <param name="action"></param>
        void Apply(Action action);
    }
}