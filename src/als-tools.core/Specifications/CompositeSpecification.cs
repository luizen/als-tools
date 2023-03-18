namespace AlsTools.Core.Specifications;

public abstract class CompositeSpecification<T> : ISpecification<T>
{
    protected readonly ISpecification<T>[] _specifications;

    protected CompositeSpecification(params ISpecification<T>[] specifications)
    {
        _specifications = specifications;
    }

    public abstract Expression<Func<T, bool>> ToExpression();
}