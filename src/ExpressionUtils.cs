using System.Linq.Expressions;

namespace BooleanLambdaBuilderDotnet;

public class ExpressionUtils
{
    public static Func<Expression, BinaryExpression> JoinExpressions(
        Func<Expression, Expression> func1,
        Func<Expression, Expression> func2,
        Func<Expression, Expression, BinaryExpression> combiner)
    {
        // Define a new function that combines the results of the two input functions with OR
        return (expr1) =>
        {
            // Invoke both functions and create a BinaryExpression with OR operation
            var result1 = func1(expr1);
            var result2 = func2(expr1);
            return combiner(result1, result2);
        };
    }

    public static Func<Expression, Expression> StringContains(string constantValue, StringComparison stringComp = StringComparison.Ordinal)
    {
        return (Expression parameter) =>
        {
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string), typeof(StringComparison) });
            var constant = Expression.Constant(constantValue);
            var comparison = Expression.Constant(stringComp);
            var containsCall = Expression.Call(parameter, containsMethod, constant, comparison);
            return containsCall;
        };
    }


    public static Func<Expression, Expression> ListContains<T>(T constantValue)
    {
        return (Expression parameter) =>
        {
            var containsMethod = typeof(Enumerable).GetMethods()
                .FirstOrDefault(m => m.Name == "Contains" && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T));
            var constant = Expression.Constant(constantValue);
            var containsCall = Expression.Call(containsMethod, parameter, constant);
            return containsCall;
        };
    }
}