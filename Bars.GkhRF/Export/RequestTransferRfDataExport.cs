namespace Bars.GkhRf.Export
{
    using System;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using B4;
    using B4.Utils;
    using B4.Modules.DataExport.Domain;
    using Entities;

    public class RequestTransferRfDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var municipalities = baseParams.Params.ContainsKey("municipalities") ? baseParams.Params["municipalities"].ToString() : string.Empty;

            List<long> checkedMunicipalities = null;

            if (!string.IsNullOrEmpty(municipalities))
            {
                checkedMunicipalities = municipalities.Split(';').Select(x => x.ToLong()).ToList();
            }

            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);

            return Container.Resolve<IDomainService<ViewRequestTransferRf>>().GetAll()
                    .WhereIf(checkedMunicipalities != null, x => x.MunicipalityId.HasValue && checkedMunicipalities.Contains(x.MunicipalityId.Value))
                    .WhereIf(dateStart != DateTime.MinValue, x => x.DateFrom >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.DateFrom <= dateEnd)
                    .Select(x => new
                        {
                            x.Id,
                            x.State,
                            x.DocumentNum,
                            x.DateFrom,
                            x.TypeProgramRequest,
                            x.MunicipalityName,
                            x.ManagingOrganizationName,
                            x.TransferFundsCount,
                            x.TransferFundsSum
                        })
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.MunicipalityName)
                    .Filter(loadParams, Container)
                    .Order(loadParams)
                    .ToList();
        }
    }
}