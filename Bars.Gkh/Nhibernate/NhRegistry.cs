namespace Bars.Gkh.Nhibernate
{
    using Bars.B4.Application;
    using Bars.B4.IoC;

    using NHibernate.Linq.Functions;

    /// <summary>
    /// Реестр генераторов HQL
    /// </summary>
    public class NhRegistry : DefaultLinqToHqlGeneratorsRegistry
    {
        public NhRegistry()
        {
            var container = ApplicationContext.Current.Container;

            container.UsingForResolvedAll<IGkhMethodHqlGenerator>((ioc, gens) =>
            {
                foreach (var hqlGenerator in gens)
                {
                    this.RegisterGenerator(hqlGenerator);
                    this.Merge(hqlGenerator);
                }
            });

            container.UsingForResolvedAll<IHqlGeneratorForProperty>((ioc, gens) =>
            {
                foreach (var hqlGenerator in gens)
                {
                    this.Merge(hqlGenerator);
                }
            });
        }
    }
}