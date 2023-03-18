namespace AlsTools.Core.Specifications;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> ToExpression();
}