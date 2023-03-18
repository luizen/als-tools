using System.Linq.Expressions;
using AlsTools.Core.Extensions;

namespace AlsTools.Core.Specifications;

public class OrSpecification<T> : CompositeSpecification<T>
{
    public OrSpecification(params ISpecification<T>[] specifications) : base(specifications) { }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var parameter = Expression.Parameter(typeof(T));
        var body = _specifications.Select(s => s.ToExpression().Body.ReplaceParameter(s.ToExpression().Parameters[0], parameter)).Aggregate(Expression.OrElse);
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}