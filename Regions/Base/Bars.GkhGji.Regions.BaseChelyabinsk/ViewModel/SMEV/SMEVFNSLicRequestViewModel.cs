namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System;
    using Enums;
    using System.Linq;

    public class SMEVFNSLicRequestViewModel : BaseViewModel<SMEVFNSLicRequest>
    {
        public override IDataResult List(IDomainService<SMEVFNSLicRequest> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x=> new
                {
                    x.Id,
                    RequestDate = x.ObjectCreateDate,
                    Inspector = x.Inspector.Fio,
                    x.RequestState,
                    x.FNSLicRequestType,
                    x.FNSLicPersonType,
                    x.CalcDate,
                    x.IdDoc
                })               
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
