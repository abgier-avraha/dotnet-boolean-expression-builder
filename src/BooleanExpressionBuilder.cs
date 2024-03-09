using System.Linq.Expressions;

namespace BooleanLambdaBuilderDotnet;

public class BooleanExpressionBuilder<T, R>
{
    private Expression<Func<T, R>> selector;
    private ExpressionNode expressionNode;
    private bool invert = false;

    public BooleanExpressionBuilder(Expression<Func<T, R>> selector)
    {
        this.selector = selector;
    }

    public BooleanExpressionBuilder<T, R> Configure(ExpressionNode expressionNode)
    {
        this.expressionNode = expressionNode;
        return this;
    }

    public Func<T, bool> ToLambda()
    {
        var param = Expression.Parameter(typeof(T), "x");
        var selectorBody = Expression.Invoke(this.selector, param);
        var comparisonExpr = this.expressionNode.ExpressionFunction(selectorBody);
        var lambda = Expression.Lambda<Func<T, bool>>(this.invert ? Expression.Not(comparisonExpr) : comparisonExpr, param);
        return lambda.Compile();
    }

    public BooleanExpressionBuilder<T, R> Not()
    {
        this.invert = true;
        return this;
    }
}
