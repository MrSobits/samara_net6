namespace Bars.GkhGji.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ResolutionDisputeViewModel : BaseViewModel<ResolutionDispute>
    {
        public override IDataResult List(IDomainService<ResolutionDispute> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToLong()
                                   : 0;

            var data = domainService.GetAll()
                .Where(x => x.Resolution.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    x.DocumentNum,
                    x.Description,
                    x.Appeal,
                    Court = x.Court.Name,
                    Instance = x.Instance.Name,
                    CourtVerdict = x.CourtVerdict.Name,
                    Lawyer = x.Lawyer.Fio,
                    x.File
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}