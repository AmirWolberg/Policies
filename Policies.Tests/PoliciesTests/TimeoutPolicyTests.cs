using Microsoft.Extensions.Logging;
using Moq;
using Policies.Policies;
using ITimer = Policies.Policies.ITimer;

namespace Policies.Tests.PoliciesTests;

public class TimeoutPolicyTests
{
     private static IEnumerable<TestCaseData> _testCaseData = new List<TestCaseData>
     {
         new (new TimeSpan[]{new (1)}, new TimeSpan(0), 1),
         new (new TimeSpan[]{new (1)}, new TimeSpan(1), 1),
         new (new TimeSpan[]{new (1), new(2)}, new TimeSpan(2), 2),
         new (new TimeSpan[]{new (1), new (2), new (3)}, new TimeSpan(3), 3),
         new (new TimeSpan[]{new (1), new (2), new (3)}, new TimeSpan(2), 2),
         new (new TimeSpan[]{new (1), new (2), new (3), new(4)}, new TimeSpan(1), 1),
     };
    
    [Test, TestCaseSource(nameof(_testCaseData))]
    public void 
        TestApplyWithEnumerableAndFunc_CreatePolicyWithMockTimerAndCallApply_ShouldCheckTimesExpectedAmountOfTimesBeforeCompleting
        (TimeSpan[] elapsedTimes, TimeSpan timeout, int timesBeforeTimeoutCount)
    {
        // Arrange
        var enumerable = new List<int>();
        for(var _ = 0; _ < elapsedTimes.Length; _++)
            enumerable.Add(0);
        var elapsedTimeIndex = 0;
        var mockTimer = new Mock<ITimer>();
        mockTimer.Setup(t => t.Elapsed)
            .Returns(() => elapsedTimes[elapsedTimeIndex++]);
        var timeoutPolicy = new TimeoutPolicy(timeout, mockTimer.Object);
        
        // Act
        foreach(var _ in timeoutPolicy.Apply(enumerable, item => item));
        
        // Assert
        mockTimer.Verify(t => t.Elapsed, Times.Exactly(timesBeforeTimeoutCount));
    }
    
    [Test, TestCaseSource(nameof(_testCaseData))]
    public void 
        TestApplyWithEnumerableAndAction_CreatePolicyWithMockTimerAndCallApply_ShouldCheckTimesExpectedAmountOfTimesBeforeCompleting
        (TimeSpan[] elapsedTimes, TimeSpan timeout, int timesBeforeTimeoutCount)
    {
        // Arrange
        var enumerable = new List<int>();
        for(var _ = 0; _ < elapsedTimes.Length; _++)
            enumerable.Add(0);
        var elapsedTimeIndex = 0;
        var mockTimer = new Mock<ITimer>();
        mockTimer.Setup(t => t.Elapsed)
            .Returns(() => elapsedTimes[elapsedTimeIndex++]);
        var timeoutPolicy = new TimeoutPolicy(timeout, mockTimer.Object);
        
        // Act
        timeoutPolicy.Apply(enumerable, _ => { Globals.Logger.LogDebug("Performed Action"); });
        
        // Assert
        mockTimer.Verify(t => t.Elapsed, Times.Exactly(timesBeforeTimeoutCount));
    }
    
    [Test, TestCaseSource(nameof(_testCaseData))]
    public void 
        TestApplyWithFunc_CreatePolicyWithMockTimerAndCallApply_ShouldCheckTimesExpectedAmountOfTimesBeforeCompleting
        (TimeSpan[] elapsedTimes, TimeSpan timeout, int timesBeforeTimeoutCount)
    {
        // Arrange
        var elapsedTimeIndex = 0;
        var mockTimer = new Mock<ITimer>();
        mockTimer.Setup(t => t.Elapsed)
            .Returns(() => elapsedTimes[elapsedTimeIndex++]);
        var timeoutPolicy = new TimeoutPolicy(timeout, mockTimer.Object);
        
        // Act
        foreach(var _ in timeoutPolicy.Apply(() => 0));
        
        // Assert
        mockTimer.Verify(t => t.Elapsed, Times.Exactly(timesBeforeTimeoutCount));
    }
    
    [Test, TestCaseSource(nameof(_testCaseData))]
    public void 
        TestApplyWithAction_CreatePolicyWithMockTimerAndCallApply_ShouldCheckTimesExpectedAmountOfTimesBeforeCompleting
        (TimeSpan[] elapsedTimes, TimeSpan timeout, int timesBeforeTimeoutCount)
    {
        // Arrange
        var elapsedTimeIndex = 0;
        var mockTimer = new Mock<ITimer>();
        mockTimer.Setup(t => t.Elapsed)
            .Returns(() => elapsedTimes[elapsedTimeIndex++]);
        var timeoutPolicy = new TimeoutPolicy(timeout, mockTimer.Object);
        
        // Act
        timeoutPolicy.Apply(() => { Globals.Logger.LogDebug("Performed Action"); });
        
        // Assert
        mockTimer.Verify(t => t.Elapsed, Times.Exactly(timesBeforeTimeoutCount));
    }
}