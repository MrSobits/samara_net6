namespace Bars.GisIntegration.Base.Service.Impl
{
    using System;

    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Entities;

    using Castle.Windsor;

    public class DataSupplierContext : IDisposable
    {
        [ThreadStatic]
        private static DataSupplierContext current;

        public static DataSupplierContext Current => DataSupplierContext.current;

        public RisContragent DataSupplier { get; }

        public bool IsDelegacy { get; }

        private IWindsorContainer Container => ApplicationContext.Current.Container;

        public DataSupplierContext(long risContragentId)
        {
            if (DataSupplierContext.current != null)
            {
                throw new InvalidOperationException("DataSupplierContext");
            }

            var risContragentDomain = this.Container.ResolveDomain<RisContragent>();
            var dataSupplierProvider = this.Container.Resolve<IDataSupplierProvider>();
            try
            {
                this.DataSupplier = risContragentDomain.Get(risContragentId);
                if (this.DataSupplier != null)
                {
                    this.IsDelegacy = !dataSupplierProvider.IsDelegacy(this.DataSupplier);
                }

                DataSupplierContext.current = this;
            }
            finally
            {
                this.Container.Release(risContragentDomain);
                this.Container.Release(dataSupplierProvider);
            }
        }

        public void Dispose()
        {
            DataSupplierContext.current = null;
        }
    }
}