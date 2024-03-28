namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Voronezh.Entities;
    using System;
    using System.Linq;

    public class ERKNMViewModel : BaseViewModel<ERKNM>
    {
        public override IDataResult List(IDomainService<ERKNM> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var dateStart = baseParams.Params.GetAs("dateStart", new DateTime());
            var dateEnd = baseParams.Params.GetAs("dateEnd", new DateTime());

            var data = domainService.GetAll()
                   .Where(x => x.RequestDate >= dateStart && x.RequestDate <= dateEnd)
                   .Select(x => new
                   {
                       x.Id,
                       x.RequestDate,
                       x.Answer,
                       Inspector = x.Inspector.Fio,
                       x.RequestState,
                       x.MessageId,
                       ERPID = x.ERPID.Trim(),
                       x.ERKNMDocumentType,
                       x.Disposal,
                       x.AppealCitsAdmonition,
                       x.ERPInspectionType,
                       x.GisErpRequestType,
                       x.KindKND,
                       x.Goals
                   })
               .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.RequestDate,
                    x.Answer,
                    x.Inspector,
                    x.RequestState,
                    x.MessageId,
                    x.ERPID,
                    x.ERKNMDocumentType,
                    Disposal = GetDocData(x.Disposal, x.AppealCitsAdmonition),
                    x.ERPInspectionType,
                    x.GisErpRequestType,
                    x.KindKND,
                    x.Goals
                }).AsQueryable()
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
        private string GetDocData(DocumentGji disp, AppealCitsAdmonition admon)
        {
            if (disp != null)
            {
                if (disp.DocumentDate.HasValue)
                    return $"{disp.DocumentNumber} от {disp.DocumentDate.Value.ToString("dd.MM.yyyy")}";
            }
            else if (admon != null)
            {
                if (admon.DocumentDate.HasValue)
                {
                    return $"{admon.DocumentNumber} от {admon.DocumentDate.Value.ToString("dd.MM.yyyy")}";
                }
            }
            return "";
        }

    }
}
