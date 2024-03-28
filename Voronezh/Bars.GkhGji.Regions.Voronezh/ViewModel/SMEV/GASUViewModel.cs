namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using B4;
    using Entities;
    using System;
    using System.Linq;

    public class GASUViewModel : BaseViewModel<GASU>
    {
        public override IDataResult List(IDomainService<GASU> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
         
            var data = domainService.GetAll()
                   .Select(x => new
                   {
                       x.Id,
                       x.RequestDate,
                       x.Answer,
                       Inspector = x.Inspector.Fio,
                       x.RequestState,
                       x.MessageId,
                       x.GasuMessageType

                   })           
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }


     
    }
}
