namespace Bars.GkhGji.Regions.Tatarstan.ViewModel
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Bars.B4.IoC;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.DomainService;
    using Entities;

    public class GisChargeViewModel : BaseViewModel<GisChargeToSend>
    {
        public override IDataResult List(IDomainService<GisChargeToSend> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var onlyUnsend = baseParams.Params.GetAs<bool>("onlyUnsend");

            var config = this.Container.Resolve<IGjiTatParamService>();

            using (Container.Using(config))
            {
                var enableLog = config.GetConfig().GetAs<bool>("GisGmpLogEnable");

                return domainService.GetAll()
                    .WhereIf(onlyUnsend, x => !x.IsSent)
                    .Select(x => new
                    {
                        x.Id,
                        x.Document.DocumentNumber,
                        x.Document.DocumentDate,
                        x.DateSend,
                        x.IsSent,
                        x.JsonObject,
                        SendLog = enableLog ? x.SendLog : null
                    })
                    .ToListDataResult(loadParams, this.Container);
            }
        }
    }
}