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

    public class ActCheckDataProvider : DocumentGjiDataProvider<ActCheckProxy>
    {
        public ActCheckDataProvider(IWindsorContainer container)
            : base(container)
        {
        }

        protected override IQueryable<ActCheckProxy> GetDataInternal(BaseParams baseParams)
        {
            var actCheckDomain = Container.Resolve<IDomainService<ActCheck>>();

            using (Container.Using(actCheckDomain))
            {
                var documentId = baseParams.Params.GetAs<long>("DocumentId");
                var actCheck = actCheckDomain.FirstOrDefault(x => x.Id == documentId);
                if (actCheck == null)
                {
                    throw new ReportProviderException("Не удалось получить акт проверки");
                }

                var actCheckProxy = new ActCheckProxy();
                FillCommonFields(actCheck, actCheckProxy);

                actCheckProxy.Area = actCheck.Area.GetValueOrDefault();
                actCheckProxy.Time = actCheck.ObjectCreateDate.ToShortTimeString();

                return new[] { actCheckProxy }.AsQueryable();
            }
        }

        public override string Name
        {
            get { return "Акт проверки"; }
        }

        public override string Description
        {
            get { return "Акт проверки"; }
        }
    }
}
