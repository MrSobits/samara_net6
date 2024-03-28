namespace Bars.GkhGji.DataProviders
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using B4.Modules.Reports;

    using Bars.GkhGji.Contracts.Meta;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class DisposalGjiDataProvider : DocumentGjiDataProvider<DisposalProxy>
    {
        public DisposalGjiDataProvider(IWindsorContainer container)
            : base(container)
        {
        }

        protected override IQueryable<DisposalProxy> GetDataInternal(BaseParams baseParams)
        {
            var disposalDomain = Container.Resolve<IDomainService<Disposal>>();

            using (Container.Using(disposalDomain))
            {
                var documentId = baseParams.Params.GetAs<long>("DocumentId");

                var disposal = disposalDomain.FirstOrDefault(x => x.Id == documentId);
                if (disposal == null)
                {
                    throw new ReportProviderException("Не удалось получить приказ");
                }

                var disposalProxy = new DisposalProxy();

                FillCommonFields(disposal, disposalProxy);

                FillRegionFields(disposal, disposalProxy);

                return new[] { disposalProxy }.AsQueryable();
            }
        }

        protected virtual void FillRegionFields(Disposal disposal, DisposalProxy disposalProxy)
        {
        }

        public override string Name
        {
            get { return "Приказ"; }
        }

        public override string Description
        {
            get { return "Приказ"; }
        }
    }
}