namespace Bars.GkhCr.ViewModel.ObjectCr
{
    using System;
    using System.Linq;
    using B4;
    using Entities;
    using Gkh.Domain;

    public class ProtocolCrTypeWorkViewModel : BaseViewModel<ProtocolCrTypeWork>
    {
        public override IDataResult List(IDomainService<ProtocolCrTypeWork> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var objectCrId = baseParams.Params.GetAsId("objectCrId");

            var data = domainService.GetAll()
                .Where(x => x.Protocol.ObjectCr.Id == objectCrId)
                .Select(x => new
                {
                    x.Id,
                    x.Protocol,
                    x.TypeWork,
                    FinanceSourceName = x.Protocol.TypeWork.FinanceSource.Name,
                    WorkName = x.TypeWork.Work.Name
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}
