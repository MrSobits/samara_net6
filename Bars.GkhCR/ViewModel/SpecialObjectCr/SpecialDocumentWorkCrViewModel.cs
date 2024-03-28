namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Gkh.Domain;

    public class SpecialDocumentWorkCrViewModel : BaseViewModel<SpecialDocumentWorkCr>
    {
        public override IDataResult List(IDomainService<SpecialDocumentWorkCr> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var objectCrId = baseParams.Params.GetAsId("objectCrId");
            var twId = baseParams.Params.GetAsId("twId");

            if (objectCrId == 0)
            {
                objectCrId = loadParams.Filter.GetAsId("objectCrId");
            }

            if (twId == 0)
            {
                twId = loadParams.Filter.GetAsId("twId");
            }

            var data = domainService.GetAll()
                .Where(x => x.ObjectCr.Id == objectCrId)
                .WhereIf(twId > 0, x => x.TypeWork.Id == twId)
                .Select(x => new
                {
                    x.Id,
                    ContragentName = x.Contragent.Name,
                    x.DocumentName,
                    x.DocumentNum,
                    x.DateFrom,
                    x.Description,
                    x.File
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}