using Bars.B4;
using Bars.Gkh.Regions.Tatarstan.Entities.Administration;
using Bars.Gkh.Utils;
using System;
using System.Linq;

namespace Bars.Gkh.Regions.Tatarstan.ViewModel.Administration
{
    public class TariffDataIntegrationLogViewModel : BaseViewModel<TariffDataIntegrationLog>
    {
        public override IDataResult List(IDomainService<TariffDataIntegrationLog> domainService, BaseParams baseParams)
        {
            var beginDate = baseParams.Params.GetAs<DateTime>("beginDate").Date;
            var endDate = baseParams.Params.GetAs<DateTime>("endDate").Date;

            return domainService.GetAll()
                .Where(x => x.StartMethodTime.Date >= beginDate && x.StartMethodTime.Date <= endDate)
                .Select(x => new
                {
                    x.Id,
                    x.TariffDataIntegrationMethod,
                    x.User.Login,
                    x.StartMethodTime,
                    x.Parameters,
                    x.ExecutionStatus,
                    LogFileId = x.LogFile != null ? x.LogFile.Id : default(long?)
                })
                .ToListDataResult(baseParams.GetLoadParam());
        }
    }
}