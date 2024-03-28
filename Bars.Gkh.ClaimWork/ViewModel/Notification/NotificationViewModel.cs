namespace Bars.Gkh.ClaimWork.ViewModel
{
    using System;
    using System.Linq;
    using B4;
    using B4.Utils;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Modules.ClaimWork.Entity;
    using Bars.Gkh.Utils;

    using Modules.ClaimWork.Entities;

    public class NotificationViewModel : BaseViewModel<NotificationClw>
    {
        public override IDataResult List(IDomainService<NotificationClw> domain, BaseParams baseParams)
        {
            var viewDocumentRepository = this.Container.ResolveRepository<ViewDocumentClw>();
            using (this.Container.Using(viewDocumentRepository))
            {
                var loadParams = this.GetLoadParam(baseParams);
                var dateStart = baseParams.Params.GetAs<DateTime?>("dateStart");
                var dateEnd = baseParams.Params.GetAs<DateTime?>("dateEnd");
                var roId = baseParams.Params.GetAs<long>("address");

                return viewDocumentRepository.GetAll()
                    .Where(x => x.DocumentType == ClaimWorkDocumentType.Notification)
                    .WhereIf(dateStart.HasValue, x => x.DocumentDate.Value.Date >= dateStart.Value.Date)
                    .WhereIf(dateEnd.HasValue, x => x.DocumentDate.Value.Date <= dateEnd.Value.Date)
                    .WhereIf(roId != 0, x => x.RealityObjectId == roId)
                    .ToListDataResult(loadParams);
            }
        }
    }
}