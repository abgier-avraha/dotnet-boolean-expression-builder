# Dotnet Boolean Expression Builder

A tool to build expressions for a given selector.


```csharp
var expr = new BooleanExpressionBuilder<User, string>(a => a.Name)
    .Configure(new ExpressionNode((selected) => Expression.Equal(selected, Expression.Constant("John")))
        .Or(new ExpressionNode((selected) => Expression.Equal(selected, Expression.Constant("Jen")))))
    .ToLambda();

var computedList = list.Where(expr);
```