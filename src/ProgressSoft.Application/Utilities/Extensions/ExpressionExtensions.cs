using System.Linq.Expressions;

namespace ProgressSoft.Application.Utilities.Extensions;

public static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        // 1. Create a parameter expression for the left body
        ParameterExpression parameter = left.Parameters[0];

        // 2. Replace the parameter in the right body to match the left
        ParameterReplaceVisitor visitor = new(right.Parameters[0], parameter);
        Expression rightBody = visitor.Visit(right.Body);

        // 3. Combine the bodies with an AND expression
        BinaryExpression combined = Expression.AndAlso(left.Body, rightBody);

        // 4. Create the final lambda expression
        return Expression.Lambda<Func<T, bool>>(combined, parameter);
    }

    // Helper class to replace parameters in an expression tree
    private class ParameterReplaceVisitor(ParameterExpression oldParameter, ParameterExpression newParameter) : ExpressionVisitor
    {
        private readonly ParameterExpression _oldParameter = oldParameter;
        private readonly ParameterExpression _newParameter = newParameter;

        protected override Expression VisitParameter(ParameterExpression node)
        {
            // Replace the old parameter with the new one
            return node == _oldParameter ? _newParameter : base.VisitParameter(node);
        }
    }
}