namespace Bars.Gkh.Nhibernate
{
    using System.Collections.Generic;
    using System.Reflection;

    using NHibernate.Linq.Functions;

    public interface IGkhMethodHqlGenerator : IHqlGeneratorForMethod, IRuntimeMethodHqlGenerator
    {
        ICollection<MethodInfo> AllowedMethods { get; }
    }
}