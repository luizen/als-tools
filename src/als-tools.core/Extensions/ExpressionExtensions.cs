namespace AlsTools.Core.Extensions;

public static class ExpressionExtensions
{
    public static Expression ReplaceParameter(this Expression expression, ParameterExpression source, ParameterExpression target)
    {
        return new ParameterReplacer(source, target).Visit(expression);
    }

    private class ParameterReplacer : ExpressionVisitor
    {
        public ParameterReplacer(ParameterExpression source, ParameterExpression target)
        {
            Source = source;
            Target = target;
        }

        public ParameterExpression Source { get; set; }

        public ParameterExpression Target { get; set; }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == Source ? Target : base.VisitParameter(node);
        }
    }
}