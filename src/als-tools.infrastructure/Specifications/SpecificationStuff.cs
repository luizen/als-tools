// using System.Linq.Expressions;
// using AlsTools.Core.Entities;
// // using Raven.Client.Documents.Linq;

// namespace AlsTools.Infrastructure.Specifications;


// public interface ISpecification<T>
// {
//     Expression<Func<T, bool>> ToExpression();
// }

// public class ProjectNameSpecification : ISpecification<LiveProject>
// {
//     private readonly string name;

//     public ProjectNameSpecification(string name)
//     {
//         this.name = name;
//     }

//     public Expression<Func<LiveProject, bool>> ToExpression()
//     {
//         return x => x.Name == name;
//     }

// }



// public interface ISpecification<T>
// {
//     bool IsSatisfiedBy(T candidate);
// }

// public abstract class Specification<T>
// {
//     // public abstract bool IsSatisfiedBy(T entity);
//     public abstract Expression<Func<T, bool>> ToExpression();
// }

// public class DeviceNameSpecification : Specification<BaseDevice>
// {
//     private readonly string name;

//     public DeviceNameSpecification(string name)
//     {
//         this.name = name;
//     }

//     public override Expression<Func<BaseDevice, bool>> ToExpression()
//     {
//         return user => user.Name == name;
//     }
// }

// public class ProjectNameSpecification : Specification<LiveProject>
// {
//     private readonly string name;

//     public ProjectNameSpecification(string name)
//     {
//         this.name = name;
//     }

//     public override Expression<Func<LiveProject, bool>> ToExpression()
//     {
//         return x => x.Name == name;
//     }
// }

// public class ProjectNameSpecification : ISpecification<LiveProject>
// {
//     private readonly string name;

//     public ProjectNameSpecification(string name)
//     {
//         this.name = name;
//     }

//     public bool IsSatisfiedBy(LiveProject candidate)
//     {
//         return candidate.Name == name;
//     }
// }

// // ==> FUNCIONA
// public interface ISpecification<T>
// {
//     IRavenQueryable<T> SatisfyingElementsFrom(IRavenQueryable<T> candidates);
// }

// public class ProjectNameSpecification : ISpecification<LiveProject>
// {
//     private readonly string name;

//     public ProjectNameSpecification(string name)
//     {
//         this.name = name;
//     }

//     public IRavenQueryable<LiveProject> SatisfyingElementsFrom(IRavenQueryable<LiveProject> candidates)
//     {
//         return candidates.Where(x => x.Name == name);
//     }
// }

// public class ProjectMajorVersionSpecification : ISpecification<LiveProject>
// {
//     private readonly string majorVersion;

//     public ProjectMajorVersionSpecification(string majorVersion)
//     {
//         this.majorVersion = majorVersion;
//     }

//     public IRavenQueryable<LiveProject> SatisfyingElementsFrom(IRavenQueryable<LiveProject> candidates)
//     {
//         return candidates.Where(x => x.MajorVersion == majorVersion);
//     }
// }

// public class AndSpecification<T> : ISpecification<T>
// {
//     private readonly ISpecification<T> first, second;

//     public AndSpecification(ISpecification<T> first, ISpecification<T> second)
//     {
//         this.first = first;
//         this.second = second;
//     }

//     public IRavenQueryable<T> SatisfyingElementsFrom(IRavenQueryable<T> candidates)
//     {
//         var firstPass = first.SatisfyingElementsFrom(candidates);
//         return second.SatisfyingElementsFrom(firstPass);
//     }
// }

// // ==> FUNCIONA


// public class ProjectMajorVersionSpecification : ISpecification<LiveProject>
// {
//     private readonly string majorVersion;

//     public ProjectMajorVersionSpecification(string majorVersion)
//     {
//         this.majorVersion = majorVersion;
//     }

//     public Expression<Func<LiveProject, bool>> ToExpression()
//     {
//         return x => x.MajorVersion == majorVersion;
//     }
// }


// public class CompositeSpecification<T> : ISpecification<T>
// {
//     public ISpecification<T> Left { get; }
//     public ISpecification<T> Right { get; }
//     public bool IsAnd { get; }

//     public CompositeSpecification(ISpecification<T> left, ISpecification<T> right, bool isAnd)
//     {
//         Left = left;
//         Right = right;
//         IsAnd = isAnd;
//     }

//     public bool IsSatisfiedBy(T candidate)
//     {
//         if (IsAnd)
//         {
//             return Left.IsSatisfiedBy(candidate) && Right.IsSatisfiedBy(candidate);
//         }
//         else
//         {
//             return Left.IsSatisfiedBy(candidate) || Right.IsSatisfiedBy(candidate);
//         }
//     }
// }

// public class CompositeSpecification<T> : ISpecification<T>
// {
//     public ISpecification<T> Left { get; }
//     public ISpecification<T> Right { get; }
//     public bool IsAnd { get; }

//     public CompositeSpecification(ISpecification<T> left, ISpecification<T> right, bool isAnd)
//     {
//         Left = left;
//         Right = right;
//         IsAnd = isAnd;
//     }

//     public bool IsSatisfiedBy(T candidate)
//     {
//         if (IsAnd)
//         {
//             return Left.IsSatisfiedBy(candidate) && Right.IsSatisfiedBy(candidate);
//         }
//         else
//         {
//             return Left.IsSatisfiedBy(candidate) || Right.IsSatisfiedBy(candidate);
//         }
//     }
// }


// public class CompositeSpecification<T> : Specification<T>
// {
//     private readonly Specification<T> left;
//     private readonly Specification<T> right;
//     private readonly Func<Expression<Func<T, bool>>, Expression<Func<T, bool>>, Expression<Func<T, bool>>> combine;

//     public CompositeSpecification(Specification<T> left, Specification<T> right, Func<Expression<Func<T, bool>>, Expression<Func<T, bool>>, Expression<Func<T, bool>>> combine)
//     {
//         this.left = left;
//         this.right = right;
//         this.combine = combine;
//     }

//     public override Expression<Func<T, bool>> ToExpression()
//     {
//         var leftExpression = left.ToExpression();
//         var rightExpression = right.ToExpression();
//         return combine(leftExpression, rightExpression);
//     }
// }



// public abstract class Specification<T>
// {
//     public abstract bool IsSatisfiedBy(T entity);
// }

// public class DeviceNameSpecification : Specification<BaseDevice>
// {
//     private readonly string name;

//     public DeviceNameSpecification(string name)
//     {
//         this.name = name;
//     }

//     public override bool IsSatisfiedBy(BaseDevice device)
//     {
//         return device.Name == name;
//     }
// }

// public class CompositeSpecification<T> : Specification<T>
// {
//     protected readonly List<Specification<T>> specifications = new List<Specification<T>>();

//     public void Add(Specification<T> specification)
//     {
//         specifications.Add(specification);
//     }

//     public void Remove(Specification<T> specification)
//     {
//         specifications.Remove(specification);
//     }

//     public override bool IsSatisfiedBy(T obj)
//     {
//         foreach (var specification in specifications)
//         {
//             if (!specification.IsSatisfiedBy(obj))
//             {
//                 return false;
//             }
//         }

//         return true;
//     }
// }


// public class AndSpecification<T> : CompositeSpecification<T>
// {
//     public override bool IsSatisfiedBy(T entity)
//     {
//         return specifications.All(spec => spec.IsSatisfiedBy(entity));
//     }
// }

// public class OrSpecification<T> : CompositeSpecification<T>
// {
//     public override bool IsSatisfiedBy(T entity)
//     {
//         return specifications.Any(spec => spec.IsSatisfiedBy(entity));
//     }
// }


// public abstract class CompositeSpecification<T> : Specification<T>
// {
//     public Specification<T> And(Specification<T> other)
//     {
//         return new AndSpecification<T>(this, other);
//     }

//     public Specification<T> Or(Specification<T> other)
//     {
//         return new OrSpecification<T>(this, other);
//     }

//     public Specification<T> Not(Specification<T> other)
//     {
//         return new NotSpecification<T>(other);
//     }
// }

// public class AndSpecification<T> : CompositeSpecification<T>
// {
//     private readonly Specification<T> left;
//     private readonly Specification<T> right;

//     public AndSpecification(Specification<T> left, Specification<T> right)
//     {
//         this.left = left;
//         this.right = right;
//     }

//     public override bool IsSatisfiedBy(T entity)
//     {
//         return left.IsSatisfiedBy(entity) && right.IsSatisfiedBy(entity);
//     }
// }


// public class AndSpecification<T> : CompositeSpecification<T>
// {
//     public override bool IsSatisfiedBy(T obj)
//     {
//         foreach (var specification in _specifications)
//         {
//             if (!specification.IsSatisfiedBy(obj))
//             {
//                 return false;
//             }
//         }

//         return true;
//     }
// }

// public class OrSpecification<T> : CompositeSpecification<T>
// {
//     private readonly Specification<T> left;
//     private readonly Specification<T> right;

//     public OrSpecification(Specification<T> left, Specification<T> right)
//     {
//         this.left = left;
//         this.right = right;
//     }

//     public override bool IsSatisfiedBy(T entity)
//     {
//         return left.IsSatisfiedBy(entity) || right.IsSatisfiedBy(entity);
//     }
// }


// public class NotSpecification<T> : CompositeSpecification<T>
// {
//     private readonly Specification<T> inner;

//     public NotSpecification(Specification<T> inner)
//     {
//         this.inner = inner;
//     }

//     public override bool IsSatisfiedBy(T entity)
//     {
//         return !inner.IsSatisfiedBy(entity);
//     }
// }