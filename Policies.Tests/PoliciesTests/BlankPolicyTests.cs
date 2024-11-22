using Policies.Policies;

namespace Policies.Tests.PoliciesTests;

public class BlankPolicyTests
{
    private static IEnumerable<int> _testCaseData = new List<int> { 0, 1, 5 };
    
    [Test, TestCaseSource(nameof(_testCaseData))]
    public void TestApplyWithEnumerableAndFunc_CallApply_ShouldReturnEnumerableIdenticalToGivenEnumerable(
        int numberOfItemsInEnumerable)
    {
        // Arrange
        var enumerable = new List<int>();
        for(var item = 0; item < numberOfItemsInEnumerable; item++)
            enumerable.Add(item);
        var blankPolicy = new BlankPolicy();
        
        // Act
        var returnedEnumerable = blankPolicy.Apply(enumerable, item => item);
        
        // Assert 
        CollectionAssert.AreEqual(enumerable, returnedEnumerable);
    }
    
    [Test, TestCaseSource(nameof(_testCaseData))]
    public void TestApplyWithEnumerableAndAction_CallApply_ShouldCallActionWithAllItemsInGivenEnumerable(
        int numberOfItemsInEnumerable)
    {
        // Arrange
        var enumerable = new List<int>();
        var iteratedOverEnumerable = new List<int>();
        for(var item = 0; item < numberOfItemsInEnumerable; item++)
            enumerable.Add(item);
        var blankPolicy = new BlankPolicy();
        
        // Act
        blankPolicy.Apply(enumerable, item =>
        {
            iteratedOverEnumerable.Add(item);
        });
        
        // Assert 
        CollectionAssert.AreEqual(enumerable, iteratedOverEnumerable);
    }
}