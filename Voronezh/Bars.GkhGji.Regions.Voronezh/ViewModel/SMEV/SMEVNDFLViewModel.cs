namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System;
    using Enums;
    using System.Linq;

    public class SMEVNDFLViewModel : BaseViewModel<SMEVNDFL>
    {
        public override IDataResult List(IDomainService<SMEVNDFL> domainService, BaseParams baseParams)
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
                    x.CalcDate,                   
                    Fio = string.Join(" ", x.FamilyName, x.FirstName, x.Patronymic),
                    x.BirthDate
                })               
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }


     
    }
}
