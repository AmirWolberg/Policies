using System;

namespace Policies.Policies
{
    /// <summary>
    /// A policy for regularly looping, implements <see cref="BasePolicy"/> directly with no changes.
    /// Keep in mind that applying this policy alone on a <see cref="Func{TResult}"/> or <see cref="Action"/> will cause
    /// and infinite loop
    /// </summary>
    public class BlankPolicy: BasePolicy { }
}