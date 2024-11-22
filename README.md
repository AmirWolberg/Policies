# Policies

A .NET C# package implementing the concept of a `Policy` in C# code.

A `policy` is a set of rules that dictate how and when a recurring action should be performed.
Policies can be chained together in order to apply multiple rules to the same recurring action.

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

To use a policy initialize it and invoke one its `Apply()` function overrides.

For example, the following code initializes and uses the [BlankPolicy](Policies/Policies/BlankPolicy.cs)

```csharp
var policy = new BlankPolicy();
policy.Apply(...);
```

In order to chain multiple policies together the `Extend()` function can be used.

For example, the following code chains the [CountPolicy](Policies/Policies/CountPolicy.cs) with the
[TimeoutPolicy](Policies/Policies/TimeoutPolicy.cs) to create a policy that performs a recurring action
5 times or for 10 seconds, whichever condition is met first.

```csharp
var chainedPolicy = new CountPolicy(5)
        .Extend(new TimeoutPolicy(TimeSpan.FromSeconds(10)));
chainedPolicy.Apply(...);
```

## Supported Policies

## How To Create Your Own Policy