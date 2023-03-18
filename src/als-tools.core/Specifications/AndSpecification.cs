namespace AlsTools.Core.Specifications;

public class AndSpecification<T> : CompositeSpecification<T>
{
    public AndSpecification(params ISpecification<T>[] specifications) : base(specifications) { }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var parameter = Expression.Parameter(typeof(T));
        var body = _specifications.Select(s => s.ToExpression().Body.ReplaceParameter(s.ToExpression().Parameters[0], parameter)).Aggregate(Expression.AndAlso);
        return Expression.Lambda<Func<T,bool>>(body, parameter);
    }
}