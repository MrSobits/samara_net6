namespace Bars.GkhGji.Export
{
    using System;
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class DisposalNullInspectionDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var dateStart = baseParams.Params.ContainsKey("dateStart")
                                   ? baseParams.Params["dateStart"].To<DateTime>()
                                   : DateTime.MinValue;

            var dateEnd = baseParams.Params.ContainsKey("dateEnd")
                                   ? baseParams.Params["dateEnd"].To<DateTime>()
                                   : DateTime.MaxValue;

            return Container.Resolve<IDomainService<Disposal>>().GetAll()
                .Where(x => x.TypeDisposal == TypeDisposalGji.NullInspection)
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MaxValue, x => x.DocumentDate <= dateEnd)
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    IssuedDisposal = x.IssuedDisposal.Fio,
                    ResponsibleExecution = x.ResponsibleExecution.Fio,
                    x.DocumentNum,
                    x.DocumentNumber,
                    x.DocumentDate
                })
                .Filter(loadParams, Container)
                .Order(loadParams)
                .ToList();
        }
    }
}