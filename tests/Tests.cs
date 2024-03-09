using System.Linq.Expressions;
using BooleanLambdaBuilderDotnet;

namespace BooleanLambdaBuilderDotnetTests;

public class Tests
{
    public class TestClass
    {
        public string Name { get; set; } = "";
        public int Age { get; set; } = 0;
        public List<string> Pets { get; set; } = new List<string>();
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

        var expr = new BooleanExpressionBuilder<TestClass, string>(a => a.Name)
            .Configure(new ExpressionNode((selected) => Expression.Equal(selected, Expression.Constant("<this>"))))
            .ToLambda();

        // Act
        var computedList = list.Where(expr);

        // Assert
        Assert.Equal(2, computedList.Count());
        Assert.Contains(computedList, v => v.Name == "<this>");
        Assert.DoesNotContain(computedList, v => v.Name == "<that>");
    }

    [Fact]
    public void TestStringContainsExpression()
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
                    Name = "<nope>"
                },
            };

        var expr = new BooleanExpressionBuilder<TestClass, string>(a => a.Name)
            .Configure(new ExpressionNode(ExpressionUtils.StringContains("th")))
            .ToLambda();

        // Act
        var computedList = list.Where(expr);

        // Assert
        Assert.Equal(4, computedList.Count());
        Assert.Contains(computedList, v => v.Name == "<this>");
        Assert.Contains(computedList, v => v.Name == "<that>");
        Assert.DoesNotContain(computedList, v => v.Name == "<nope>");
    }

    [Fact]
    public void TestListContainsExpression()
    {
        // Arrange
        var list = new List<TestClass>()
            {
                new TestClass()
                {
                    Pets = new List<string>(){"dog", "cat"}
                },
                new TestClass()
                {
                    Pets = new List<string>(){"dog", "cat"}
                },
                new TestClass()
                {
                    Pets = new List<string>(){"dog", "rabbit"}
                },
                new TestClass()
                {
                    Pets = new List<string>(){"frog", "cat"}
                },
                new TestClass()
                {
                    Pets = new List<string>(){"rat",}
                },
            };

        // TODO: wrap the expression configuration to make it type safe
        var expr = new BooleanExpressionBuilder<TestClass, List<string>>(a => a.Pets)
            .Configure(new ExpressionNode(ExpressionUtils.ListContains("dog")))
            .ToLambda();

        // Act
        var computedList = list.Where(expr);

        // Assert
        Assert.Equal(3, computedList.Count());
    }

    [Fact]
    public void TestStringEquivalencyOrExpression()
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
                    Name = "<nope>"
                },
            };

        var expr = new BooleanExpressionBuilder<TestClass, string>(a => a.Name)
            .Configure(new ExpressionNode((selected) => Expression.Equal(selected, Expression.Constant("<this>")))
                .Or(new ExpressionNode((selected) => Expression.Equal(selected, Expression.Constant("<that>")))))
            .ToLambda();

        // Act
        var computedList = list.Where(expr);

        // Assert
        Assert.Equal(4, computedList.Count());
        Assert.Contains(computedList, v => v.Name == "<this>");
        Assert.Contains(computedList, v => v.Name == "<that>");
        Assert.DoesNotContain(computedList, v => v.Name == "<nope>");
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

        var expr = new BooleanExpressionBuilder<TestClass, int>(a => a.Age)
            .Configure(new ExpressionNode((selected) => Expression.Equal(selected, Expression.Constant(3))))
            .ToLambda();

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


        var expr = new BooleanExpressionBuilder<TestClass, int>(a => a.Age)
            .Configure(new ExpressionNode((selected) => Expression.GreaterThanOrEqual(selected, Expression.Constant(0)))
                .And(new ExpressionNode((selected) => Expression.LessThanOrEqual(selected, Expression.Constant(8)))))
            .ToLambda();


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

        var expr = new BooleanExpressionBuilder<TestClass, int>(a => a.Age)
            .Configure(new ExpressionNode((selected) => Expression.GreaterThanOrEqual(selected, Expression.Constant(0)))
                .And(new ExpressionNode((selected) => Expression.LessThanOrEqual(selected, Expression.Constant(8)))))
            .Not()
            .ToLambda();

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