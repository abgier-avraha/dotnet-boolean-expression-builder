using System.Linq.Expressions;

namespace BooleanLambdaBuilderDotnet;

public class BooleanLambdaBuilder<T, R>
{
    private bool invert = false;
    private Expression<Func<T, R>> selector;
    private Func<Expression, Expression, BinaryExpression> comparison;
    private R comparedValue;

    public BooleanLambdaBuilder()
    {

    }

    public BooleanLambdaBuilder<T, R> Select(Expression<Func<T, R>> selector)
    {
        this.selector = selector;
        return this;
    }

    public BooleanLambdaBuilder<T, R> Compare(Func<Expression, Expression, BinaryExpression> comparison)
    {
        this.comparison = comparison;
        return this;
    }

    public BooleanLambdaBuilder<T, R> Or(Func<Expression, Expression, BinaryExpression> comparison)
    {
        this.comparison = JoinExpressions(this.comparison, comparison, Expression.Or);
        return this;
    }


    public BooleanLambdaBuilder<T, R> And(Func<Expression, Expression, BinaryExpression> comparison)
    {
        this.comparison = JoinExpressions(this.comparison, comparison, Expression.And);
        return this;
    }

    public BooleanLambdaBuilder<T, R> Xor(Func<Expression, Expression, BinaryExpression> comparison)
    {
        this.comparison = JoinExpressions(this.comparison, comparison, Expression.ExclusiveOr);
        return this;
    }

    public BooleanLambdaBuilder<T, R> Not()
    {
        this.invert = true;
        return this;
    }

    public BooleanLambdaBuilder<T, R> Against(R comparedValue)
    {
        this.comparedValue = comparedValue;
        return this;
    }

    public Func<T, bool> Build()
    {
        var param = Expression.Parameter(typeof(T), "x");
        var selectorBody = Expression.Invoke(this.selector, param);
        var comparedValueExpr = Expression.Constant(this.comparedValue, typeof(R));
        var comparisonExpr = this.comparison(selectorBody, comparedValueExpr);
        var lambda = Expression.Lambda<Func<T, bool>>(this.invert ? Expression.Not(comparisonExpr) : comparisonExpr, param);
        return lambda.Compile();
    }

    private static Func<Expression, Expression, BinaryExpression> JoinExpressions(
    Func<Expression, Expression, BinaryExpression> func1,
    Func<Expression, Expression, BinaryExpression> func2,
    Func<Expression, Expression, BinaryExpression> combiner)
    {
        // Define a new function that combines the results of the two input functions with OR
        return (expr1, expr2) =>
        {
            // Invoke both functions and create a BinaryExpression with OR operation
            var result1 = func1(expr1, expr2);
            var result2 = func2(expr1, expr2);
            return combiner(result1, result2);
        };
    }
}