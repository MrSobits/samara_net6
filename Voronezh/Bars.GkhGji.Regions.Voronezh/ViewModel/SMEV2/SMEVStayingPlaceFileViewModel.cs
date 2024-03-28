namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System.Linq;
    using System;

    public class SMEVStayingPlaceFileViewModel : BaseViewModel<SMEVStayingPlaceFile>
    {
        public override IDataResult List(IDomainService<SMEVStayingPlaceFile> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("SMEVStayingPlace", 0L);

            var data = domain.GetAll()
             .Where(x => x.SMEVStayingPlace.Id == id)
            .Select(x => new
            {
                x.Id,
                x.FileInfo,
                x.SMEVFileType
            })
            .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}