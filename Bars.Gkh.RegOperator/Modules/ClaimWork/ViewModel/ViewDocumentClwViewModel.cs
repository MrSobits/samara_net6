namespace Bars.Gkh.RegOperator.Modules.ViewModel
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Modules.ClaimWork.Entity;
    using Bars.Gkh.Utils;

    public class ViewDocumentClwViewModel : BaseViewModel<ViewDocumentClw>
    {       
        public override IDataResult List(IDomainService<ViewDocumentClw> domainService, BaseParams baseParams)
        {
            var documentType = baseParams.Params.GetAs<ClaimWorkDocumentType?>("documentType");

            if (documentType == null)
            {
                return BaseDataResult.Error("Не указан тип документа");
            }

            var dateStart = baseParams.Params.GetAs<DateTime?>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime?>("dateEnd");
            var roId = baseParams.Params.GetAs<long>("address");

            return domainService.GetAll()
                .Where(x => x.DocumentType == documentType)
                .WhereIf(documentType == ClaimWorkDocumentType.Pretension, x => x.BaseType == ClaimWorkTypeBase.Debtor)
                .WhereIf(dateStart.HasValue, x => x.DocumentDate.Value.Date >= dateStart.Value.Date)
                .WhereIf(dateEnd.HasValue, x => x.DocumentDate.Value.Date <= dateEnd.Value.Date)
                .WhereIf(roId != 0, x => x.RealityObjectId == roId)
                .ToListDataResult(baseParams.GetLoadParam());
        }
    }
}