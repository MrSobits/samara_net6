namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System;
    using Enums;
    using System.Linq;

    public class MVDPassportViewModel : BaseViewModel<MVDPassport>
    {
        public override IDataResult List(IDomainService<MVDPassport> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    ReqId = x.Id,
                    x.MessageId,
                    RequestDate = x.ObjectCreateDate,
                    Inspector = x.Inspector.Fio,
                    x.CalcDate,
                    x.RequestState,
                    x.MVDPassportRequestType,
                    RequestPerson = GetPerson(x)
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        private string GetPerson(MVDPassport obj)
        {
            switch (obj.MVDPassportRequestType)
            {
                case MVDPassportRequestType.PersonInfo:
                    return $"{obj.Surname} {obj.Name} {obj.PatronymicName}";
                case MVDPassportRequestType.RussianPassport:
                    return $"{obj.PassportSeries} {obj.PassportNumber}";
                default: return $"{obj.Surname} {obj.Name} {obj.PatronymicName}";
            }                
        }
    }
}
