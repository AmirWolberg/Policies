using Policies.Policies;

namespace Policies.Tests.PoliciesTests;

public class BlankPolicyTests
{
    [Test, TestCase(0), TestCase(1), TestCase(5)]
    public void TestApply_CallApplyFuncWithEnumerableAndFunc_ShouldReturnEnumerableIdenticalToGivenEnumerable(
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
    
    [Test, TestCase(0), TestCase(1), TestCase(5)]
    public void TestApply_CallApplyFuncWithEnumerableAndAction_ShouldCallActionWithAllItemsInGivenEnumerable(
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