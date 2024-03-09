using System.Linq.Expressions;

namespace BooleanLambdaBuilderDotnet;


public class ExpressionNode
{
    public Func<Expression, Expression> ExpressionFunction;

    public ExpressionNode(Func<Expression, Expression> expressionFunction)
    {
        this.ExpressionFunction = expressionFunction;
    }

    public ExpressionNode Or(ExpressionNode expr)
    {
        return new ExpressionNode(
            ExpressionUtils.JoinExpressions(this.ExpressionFunction, expr.ExpressionFunction, Expression.Or));
    }

    public ExpressionNode And(ExpressionNode expr)
    {
        return new ExpressionNode(
            ExpressionUtils.JoinExpressions(this.ExpressionFunction, expr.ExpressionFunction, Expression.And));

    }

    public ExpressionNode Xor(ExpressionNode expr)
    {
        return new ExpressionNode(
            ExpressionUtils.JoinExpressions(this.ExpressionFunction, expr.ExpressionFunction, Expression.ExclusiveOr));

    }

}
