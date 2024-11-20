using Policies.Policies;

namespace Policies.Tests.PoliciesTests;

public class CountPolicyTests
{
    private static IEnumerable<ulong> _testCaseData = new List<ulong> { 0, 1, 5 };
    
    [Test, TestCaseSource(nameof(_testCaseData))]
    public void TestApply_CallApplyWithEnumerableAndFunc_ShouldReturnEnumerableWithAmountElements(ulong amount)
    {
        // Arrange
        var enumerable = new List<int>();
        for(ulong _ = 0; _ < amount * 2 + 1; _++)
            enumerable.Add(0);
        var countPolicy = new CountPolicy(amount);
        
        // Act
        var returnedEnumerable = countPolicy.Apply(enumerable, item => item);
        
        // Assert 
        Assert.That(returnedEnumerable.Count(), Is.EqualTo(amount));
    }
    
    [Test, TestCaseSource(nameof(_testCaseData))]
    public void TestApply_CallApplyWithEnumerableAndAction_ShouldCallActionAmountTimes(ulong amount)
    {
        // Arrange
        var enumerable = new List<int>();
        var numberOfActionCalls = 0;
        for(ulong _ = 0; _ < amount * 2 + 1; _++)
            enumerable.Add(0);
        var countPolicy = new CountPolicy(amount);
        
        // Act
        countPolicy.Apply(enumerable, _ =>
        {
            numberOfActionCalls++;
        });
        
        // Assert 
        Assert.That(numberOfActionCalls, Is.EqualTo(amount));
    }
    
    [Test, TestCaseSource(nameof(_testCaseData))]
    public void TestApply_CallApplyWithFunc_ShouldReturnEnumerableWithAmountElements(ulong amount)
    {
        // Arrange
        var countPolicy = new CountPolicy(amount);
        
        // Act
        var returnedEnumerable = countPolicy.Apply(() => 0);
        
        // Assert 
        Assert.That(returnedEnumerable.Count(), Is.EqualTo(amount));
    }
    
    [Test, TestCaseSource(nameof(_testCaseData))]
    public void TestApply_CallApplyWithAction_ShouldCallActionAmountTimes(ulong amount)
    {
        // Arrange
        var numberOfActionCalls = 0;
        var countPolicy = new CountPolicy(amount);
        
        // Act
        countPolicy.Apply(() =>
        {
            numberOfActionCalls++;
        });
        
        // Assert 
        Assert.That(numberOfActionCalls, Is.EqualTo(amount));
    }
}