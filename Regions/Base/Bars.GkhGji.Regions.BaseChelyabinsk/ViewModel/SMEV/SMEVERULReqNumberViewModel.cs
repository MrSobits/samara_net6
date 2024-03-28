namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System;
    using Enums;
    using System.Linq;

    public class SMEVERULReqNumberViewModel : BaseViewModel<SMEVERULReqNumber>
    {
        public override IDataResult List(IDomainService<SMEVERULReqNumber> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x=> new
                {
                    x.Id,
                    RequestDate = x.ObjectCreateDate,
                    Inspector = x.Inspector.Fio,
                    x.RequestState,
                    x.ManOrgLicense.DateDisposal,
                    x.ManOrgLicense.DisposalNumber,
                    Contragent = x.ManOrgLicense.Contragent.Name,
                    Inn = x.ManOrgLicense.Contragent.Inn,
                    Ogrn = x.ManOrgLicense.Contragent.Ogrn,
                    x.ERULRequestType
                })               
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }


     
    }
}
