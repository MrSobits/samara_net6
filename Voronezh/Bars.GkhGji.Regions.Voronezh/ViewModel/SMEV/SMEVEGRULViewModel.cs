namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System;
    using Enums;
    using System.Linq;

    public class SMEVEGRULViewModel : BaseViewModel<SMEVEGRUL>
    {
        public override IDataResult List(IDomainService<SMEVEGRUL> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x=> new
                {
                    x.Id,
                    ReqId = x.Id,
                    x.MessageId,
                    RequestDate = x.ObjectCreateDate,
                    Inspector = x.Inspector.Fio,
                    x.RequestState,
                    x.INNReq,
                    x.CalcDate,

                })               
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }


     
    }
}
