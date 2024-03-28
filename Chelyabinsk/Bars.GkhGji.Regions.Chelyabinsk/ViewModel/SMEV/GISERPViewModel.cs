namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using B4;
    using Entities;
    using System;
    using System.Linq;

    public class GISERPViewModel : BaseViewModel<GISERP>
    {
        public override IDataResult List(IDomainService<GISERP> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Where(x => x.ObjectCreateDate >= DateTime.Now.AddYears(-1))
                   .Select(x => new
                   {
                       x.Id,
                       x.RequestDate,
                       x.Answer,
                       Inspector = x.Inspector.Fio,
                       x.RequestState,
                       x.MessageId,
                       x.ERPID,
                       x.RegistryDisposalNumber,
                       x.Disposal.DocumentDate,
                       x.Disposal.DocumentNumber,
                       x.ERPInspectionType,
                       x.GisErpRequestType,
                       x.KindKND,
                       x.Goals

                   })
               .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.RequestDate,
                    x.Answer,
                    x.Inspector,
                    x.ERPID,
                    x.RequestState,
                    x.MessageId,
                    x.RegistryDisposalNumber,
                    Disposal = x.DocumentDate.HasValue ? x.DocumentNumber + " от " + x.DocumentDate.Value.ToShortDateString() : x.DocumentNumber,
                    x.ERPInspectionType,
                    x.GisErpRequestType,
                    x.KindKND,
                    x.Goals
                }).AsQueryable()
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }



    }
}
