namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using B4;
    using Entities;
    using Bars.Gkh.Entities;
    using Gkh.Authentification;
    using System.Collections.Generic;
    using B4.Utils;
    using System;
    using Enums;
    using System.Linq;

    public class SSTUExportTaskViewModel : BaseViewModel<SSTUExportTask>
    {
        public override IDataResult List(IDomainService<SSTUExportTask> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    TaskDate = x.ObjectCreateDate,
                    Operator = x.Operator.User.Name,
                    x.SSTUExportState,
                    x.SSTUSource,
                    x.FileInfo

                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }


        public override IDataResult Get(IDomainService<SSTUExportTask> domain, BaseParams baseParams)
        {

            var loadParams = baseParams.GetLoadParam();
            int id = Convert.ToInt32(baseParams.Params.Get("id"));

            var data = domain.GetAll()
                 .WhereIf(id != 0, x => x.Id == id)
                   .Select(x => new
                   {
                       x.Id,
                       Operator = x.Operator.User.Name,
                       x.SSTUExportState,
                       x.SSTUSource,
                       TaskDate = x.ObjectCreateDate.ToShortDateString(),
                       x.ExportExported
                   })
                .AsQueryable()
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
