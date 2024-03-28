namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System;
    using Enums;
    using System.Linq;

    public class SMEVValidPassportViewModel : BaseViewModel<SMEVValidPassport>
    {
        public override IDataResult List(IDomainService<SMEVValidPassport> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x=> new
                {
                    x.Id,
                    ReqId = x.Id,
                    RequestDate = x.ObjectCreateDate,
                    Inspector = x.Inspector.Fio,
                    x.RequestState,
                    x.DocIssueDate,
                    x.DocSerie,
                    x.DocNumber,
                    x.CalcDate,
                    x.MessageId,
                    SerNumber = x.DocSerie + x.DocNumber
                })               
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }


     
    }
}
