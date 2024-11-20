# Policies

A .NET C# package implementing the concept of a `Policy` in C# code.

A `policy` is a set of rules that dictate how and when a recurring action should be performed.

[TOC]

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

## Supported Policies

## How To Create Your Own Policy