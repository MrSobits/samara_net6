namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System;
    using Enums;
    using System.Linq;

    public class SMEVChangePremisesStateViewModel : BaseViewModel<SMEVChangePremisesState>
    {
        public override IDataResult List(IDomainService<SMEVChangePremisesState> domainService, BaseParams baseParams)
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
                    Municipality = x.Municipality != null ? x.Municipality.Name : "",
                    x.CalcDate,                   
                    x.ChangePremisesType,
                    ChangePremisesTypeValue = x.ChangePremisesType == ChangePremisesType.Address ? x.RealityObject.FiasAddress.AddressName : x.CadastralNumber
                })
                .Filter(loadParams, Container);

                int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }


     
    }
}
