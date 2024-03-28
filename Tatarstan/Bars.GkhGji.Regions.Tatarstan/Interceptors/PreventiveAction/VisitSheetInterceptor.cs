namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.PreventiveAction
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;
    using System.Linq;

    public class VisitSheetInterceptor : DocumentGjiInterceptor<VisitSheet>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<VisitSheet> service, VisitSheet entity)
        {
            var visitSheetInfoDomain = this.Container.Resolve<IDomainService<VisitSheetInfo>>();
            var visitSheetAnnexDomain = this.Container.Resolve<IDomainService<VisitSheetAnnex>>();

            using (this.Container.Using(visitSheetInfoDomain, visitSheetAnnexDomain))
            {
                visitSheetInfoDomain.GetAll()
                    .Where(x => x.VisitSheet == entity)
                    .ForEach(x => visitSheetInfoDomain.Delete(x.Id));

                visitSheetAnnexDomain.GetAll()
                    .Where(x => x.VisitSheet == entity)
                    .ForEach(x => visitSheetAnnexDomain.Delete(x.Id));
            }

            return base.BeforeDeleteAction(service, entity);
        }
    }
}