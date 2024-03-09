using System.Linq.Expressions;
using BooleanLambdaBuilderDotnet;

namespace BooleanLambdaBuilderDotnetTests;

public class Tests
{
    public class TestClass
    {
        public string Name { get; set; } = "";
        public int Age { get; set; } = 0;
    }

    [Fact]
    public void TestStringEquivalencyExpression()
    {
        // Arrange
        var list = new List<TestClass>()
        {
            new TestClass()
            {
                Name = "<this>"
            },
            new TestClass()
            {
                Name = "<this>"
            },
            new TestClass()
            {
                Name = "<that>"
            },
            new TestClass()
            {
                Name = "<that>"
            },
            new TestClass()
            {
                Name = "<that>"
            },
        };

        var expr = new BooleanLambdaBuilder<TestClass, string>()
            .Select(a => a.Name)
            .Compare(BinaryExpression.Equal)
            .Against("<this>")
            .Build();

        // Act
        var computedList = list.Where(expr);

        // Assert
        Assert.Equal(2, computedList.Count());
        Assert.Contains(computedList, v => v.Name == "<this>");
        Assert.DoesNotContain(computedList, v => v.Name == "<that>");
    }

    [Fact]
    public void TestIntegerEquivalencyExpression()
    {
        // Arrange
        var list = new List<TestClass>()
        {
            new TestClass()
            {
                Age = 3
            },
            new TestClass()
            {
                Age = 3
            },
            new TestClass()
            {
                Age = 3
            },
            new TestClass()
            {
                Age = 6
            },
            new TestClass()
            {
                Age = 6
            },
        };

        var expr = new BooleanLambdaBuilder<TestClass, int>()
            .Select(a => a.Age)
            .Compare(BinaryExpression.Equal)
            .Against(3)
            .Build();

        // Act
        var computedList = list.Where(expr);

        // Assert
        Assert.Equal(3, computedList.Count());
        Assert.Contains(computedList, v => v.Age == 3);
        Assert.DoesNotContain(computedList, v => v.Age == 6);
    }

    [Fact]
    public void TestIntegerCustomBooleanExpression()
    {
        // Arrange
        var list = new List<TestClass>()
        {
            new TestClass()
            {
                Age = 8
            },
            new TestClass()
            {
                Age = 9
            },
            new TestClass()
            {
                Age = 6
            },
            new TestClass()
            {
                Age = 21
            },
            new TestClass()
            {
                Age = 17
            },
        };

        var expr = new BooleanLambdaBuilder<TestClass, int>()
            .Select(a => a.Age)
            // Within 5 checker
            .Compare((selected, compared) => Expression.And(
                Expression.GreaterThanOrEqual(selected, Expression.Subtract(compared, Expression.Constant(5))),
                Expression.LessThanOrEqual(selected, Expression.Add(compared, Expression.Constant(5)))))
            .Against(3)
            .Build();

        // Act
        var computedList = list.Where(expr);

        // Assert
        Assert.Equal(2, computedList.Count());
        Assert.Contains(computedList, v => v.Age == 8);
        Assert.Contains(computedList, v => v.Age == 6);
        Assert.DoesNotContain(computedList, v => v.Age == 21);
        Assert.DoesNotContain(computedList, v => v.Age == 17);
    }

    [Fact]
    public void TestIntegerCustomBooleanNotExpression()
    {
        // Arrange
        var list = new List<TestClass>()
        {
            new TestClass()
            {
                Age = 8
            },
            new TestClass()
            {
                Age = 9
            },
            new TestClass()
            {
                Age = 6
            },
            new TestClass()
            {
                Age = 21
            },
            new TestClass()
            {
                Age = 17
            },
        };

        var expr = new BooleanLambdaBuilder<TestClass, int>()
            .Select(a => a.Age)
            // Within 5 checker
            .Compare((selected, compared) => Expression.And(
                Expression.GreaterThanOrEqual(selected, Expression.Subtract(compared, Expression.Constant(5))),
                Expression.LessThanOrEqual(selected, Expression.Add(compared, Expression.Constant(5)))))
            .Against(3)
            .Not()
            .Build();

        // Act
        var computedList = list.Where(expr);

        // Assert
        Assert.Equal(3, computedList.Count());
        Assert.DoesNotContain(computedList, v => v.Age == 6);
        Assert.DoesNotContain(computedList, v => v.Age == 8);
        Assert.Contains(computedList, v => v.Age == 9);
        Assert.Contains(computedList, v => v.Age == 21);
        Assert.Contains(computedList, v => v.Age == 17);
    }
}