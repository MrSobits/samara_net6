namespace Bars.GkhCr.ViewModel.ObjectCr
{
    using System;
    using System.Linq;
    using B4;
    using Entities;
    using Gkh.Domain;

    public class ArchiveMultiplyContragentSmrViewModel : BaseViewModel<ArchiveMultiplyContragentSmr>
    {
        public override IDataResult List(IDomainService<ArchiveMultiplyContragentSmr> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var typeWorkId = baseParams.Params.GetAsId("typeWorkId");

            var data = domainService.GetAll()
                .Where(x => x.TypeWorkCr.Id == typeWorkId)
                .Select(x => new
                {
                    x.Id,
                    x.CostSum,
                    Contragent = x.Contragent.Name,
                    x.PercentOfCompletion,
                    x.VolumeOfCompletion
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}
