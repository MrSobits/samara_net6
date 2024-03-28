namespace Bars.GkhGji.Regions.Habarovsk.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System;
    using Enums;
    using System.Linq;

    public class SMEVMVDViewModel : BaseViewModel<SMEVMVD>
    {
        public override IDataResult List(IDomainService<SMEVMVD> domainService, BaseParams baseParams)
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
                    Contragent = x.Contragent != null? x.Contragent.Name:"",
                    ContragentContact = x.ContragentContact != null ? x.ContragentContact.FullName:"",
                    x.CalcDate,
                    x.RequestState,
                    RequestPerson = x.Surname + " " + x.Name + " " + x.PatronymicName,
                 
                 
                })               
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }


        public override IDataResult Get(IDomainService<SMEVMVD> domain, BaseParams baseParams)
        {

            var loadParams = baseParams.GetLoadParam();
            long id = Convert.ToInt64(baseParams.Params.Get("id"));

            var data = domain.GetAll()
                 .WhereIf(id != 0, x => x.Id == id)
                   .Select(x => new
                   {
                       x.Id,
                       Inspector = x.Inspector.Fio,
                       x.AddressAdditional,
                       x.AddressPrimary,
                       x.MessageId,
                       x.CalcDate,
                       x.BirthDate,
                       x.AnswerInfo,
                       x.MVDTypeAddressAdditional,
                       x.MVDTypeAddressPrimary,
                       x.PatronymicName,
                       x.RegionCodeAdditional,
                       x.RegionCodePrimary,
                       x.SNILS,
                       x.Surname,
                       x.Name,
                       x.Answer,
                       x.ContragentContact,
                       x.Contragent
                   })
                .AsQueryable()
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
