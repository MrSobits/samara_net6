using System.Linq;

namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{

    using B4;
    using B4.Utils;
    using Entities;
    using System;

    public class SSTUExportTaskAppealViewModel : BaseViewModel<SSTUExportTaskAppeal>
    {
        public override IDataResult List(IDomainService<SSTUExportTaskAppeal> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("SSTUExportTask", 0L);
            //var isFiltered = loadParams.Filter.GetAs("isFiltered", false);

            var data = domain.GetAll()
             .Where(x => x.SSTUExportTask.Id == id)
            .Select(x => new
            {
                x.Id,
                AppealCits = x.AppealCits.DocumentNumber,
                AppealCitsDate = x.AppealCits.DateFrom
            })
            .AsQueryable()
            .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());

        }
    }
}