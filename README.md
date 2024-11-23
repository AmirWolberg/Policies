# Policies

A .NET C# package implementing the concept of a `Policy` in C# code.

A `Policy` is a mechanism for applying conditional logic and looping behavior to recurring actions or data processing
workflows. Policies can be chained together, creating reusable and composable building blocks for complex workflows.

## Overview

The `Policies` package is a framework for managing iterative processes using customizable and chainable policies.
At its core is the [BasePolicy](Policies/BasePolicy.cs) class, which provides a foundation for implementing rules that
govern the behavior of a recurring action.
These rules can modify the behavior of the recurrence by determining the initialization steps,
conditions for applying the policy, criteria for completion, and any mutations that should occur during each iteration.

## Why Use Policies?

The Policies package is ideal for:

* **Iterative Data Processing**: Apply rules to manage processing large datasets, throttling execution,
  or skipping items based on conditions.
* **Task Automation**: Define behaviors for retry logic, time-based delays, or other iterative processes
  in automated workflows.
* **Resilient Systems**: Build robust control mechanisms for systems that need dynamic or conditional loops,
  such as in fault-tolerant systems.
* **Modular Design**: Extendable policies make it easy to reuse and compose behavior for different scenarios.

## How To Use A Policy

To use a policy initialize it and invoke one of its `Apply()` function overrides.

For example, the following code initializes and uses the [CountPolicy](Policies/Policies/CountPolicy.cs)

```csharp
var policy = new CountPolicy(3);
policy.Apply(() => Console.WriteLine("Running policy iteration!"));

// Output:
// Running policy iteration!
// Running policy iteration!
// Running policy iteration!
```

In order to chain multiple policies together the `Extend()` function can be used.

For example, the following code chains the [CountPolicy](Policies/Policies/CountPolicy.cs) with the
[TimeoutPolicy](Policies/Policies/TimeoutPolicy.cs) to create a policy that performs a recurring action
3 times or for 2 seconds, whichever condition is met first.

```csharp
var chainedPolicy = new CountPolicy(3)
        .Extend(new TimeoutPolicy(TimeSpan.FromSeconds(2)));
chainedPolicy.Apply(() =>
{
    Console.WriteLine("Processing...");
    System.Threading.Thread.Sleep(1000); // Simulate work
});

// Output:
// Processing...
// Processing...
```

## Supported Policies

All policies natively supported in this package can be found under the namespace
[Policies.Policies](Policies/Policies).

## How To Create Your Own Policy

1. inherit from [BasePolicy](Policies/BasePolicy.cs):

```csharp
public class MyCustomPolicy : BasePolicy { }
```

2. Override Key Methods:

* `Initialize()`: For setup logic.
* `ShouldApply()` / `ShouldApply<TItem>(TItem item)`: Decide whether the loop should proceed or wait.
* `Mutate()`: For modifying state during each iteration.
* `Completed()` / `Completed<TOut>(TOut output)`: For termination logic.
