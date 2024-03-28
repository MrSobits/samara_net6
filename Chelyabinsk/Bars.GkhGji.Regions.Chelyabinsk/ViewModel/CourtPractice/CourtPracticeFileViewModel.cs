namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System.Linq;
    using System;

    public class CourtPracticeFileViewModel : BaseViewModel<CourtPracticeFile>
    {
        public override IDataResult List(IDomainService<CourtPracticeFile> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("courtpracticeId", 0L);

            var data = domain.GetAll()
             .Where(x => x.CourtPractice.Id == id)
            .Select(x => new
            {
                x.Id,
                x.FileInfo,
                x.Description,
                x.DocumentName,
                x.DocDate
            })
            .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());



        }
    }
}