namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System;
    using Enums;
    using System.Linq;

    public class MVDStayingPlaceRegistrationViewModel : BaseViewModel<MVDStayingPlaceRegistration>
    {
        public override IDataResult List(IDomainService<MVDStayingPlaceRegistration> domainService, BaseParams baseParams)
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
                    RequestPerson = $"{x.Surname} {x.Name} {x.PatronymicName}"
                }).Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
 
    }
}
