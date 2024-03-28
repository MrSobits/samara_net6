namespace Bars.B4.Modules.Analytics.Executions
{
    using System.Linq;
    using Bars.B4.Application;
    using Bars.B4.Events;
    using Bars.B4.Modules.Analytics.Domain;
    using Bars.B4.Modules.Analytics.Entities;

    using Castle.MicroKernel.Lifestyle;
    using NHibernate.Exceptions;

    /// <summary>
    /// 
    /// </summary>
    public class MigrateDataProvidersHandler : EventHandlerBase<AppStartEventArgs>
    {

        public override void OnEvent(AppStartEventArgs args)
        {
            using (ApplicationContext.Current.Container.BeginScope())
            {
                var container = ApplicationContext.Current.Container;
                var dataProviders = container.Resolve<IDataProviderService>().GetAll();
                var dataSourceDomain = container.Resolve<IDomainService<DataSource>>();


                try
                {
                    var providerKeys = dataProviders.Where(x => !x.IsHidden).Select(x => x.Key).ToArray();
                    var existProvidersKeys = dataSourceDomain.GetAll().Where(x => providerKeys.Contains(x.ProviderKey))
                        .Select(x => x.ProviderKey).ToArray();
                    foreach (var provider in dataProviders)
                    {
                        if (!existProvidersKeys.Contains(provider.Key))
                        {
                            dataSourceDomain.Save(new DataSource(provider));
                        }
                    }

                }
                catch (GenericADOException)
                {
                    // TODO: Log warning
                }
                finally
                {
                    foreach (var provider in dataProviders)
                    {
                        container.Release(provider);
                    }
                    container.Release(dataSourceDomain);
                }

            }
        }
    }
}
