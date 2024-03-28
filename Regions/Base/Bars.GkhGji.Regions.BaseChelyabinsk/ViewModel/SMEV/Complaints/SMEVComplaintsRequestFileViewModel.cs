namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System.Linq;
    using System;

    public class SMEVComplaintsRequestFileViewModel : BaseViewModel<SMEVComplaintsRequestFile>
    {
        public override IDataResult List(IDomainService<SMEVComplaintsRequestFile> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("requestId", 0L);
            //var isFiltered = loadParams.Filter.GetAs("isFiltered", false);

            var data = domain.GetAll()
             .Where(x => x.SMEVComplaintsRequest.Id == id)
            .Select(x => new
            {
                x.Id,
                x.FileInfo,
                x.FileInfo.Name,
                x.SMEVFileType
            })
            .AsQueryable()
            .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());

        }
    }
}