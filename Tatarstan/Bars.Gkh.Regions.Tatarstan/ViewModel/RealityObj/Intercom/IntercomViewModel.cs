namespace Bars.Gkh.Regions.Tatarstan.ViewModel.RealityObj.Intercom
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Regions.Tatarstan.Entities.RealityObject;
    using Bars.Gkh.Utils;

    public class IntercomViewModel : BaseViewModel<Intercom>
    {
        public override IDataResult List(IDomainService<Intercom> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var objectId = baseParams.Params.GetAs<long>("objectId");

            return domainService.GetAll()
                .Where(x=>x.RealityObject.Id == objectId)
                .Select(x=> new
                {
                    x.Id,
                    x.ArchiveAccess,
                    x.ArchiveDepth,
                    x.IntercomCount,
                    x.Recording,
                    Tariff = $"{x.Tariff} {(x.UnitMeasure.HasValue ? x.UnitMeasure.GetDisplayName() : "")}",
                    x.InstallationDate,
                    x.AnalogIntercomCount,
                    x.EntranceCount,
                    x.IntercomInstallationDate,
                    x.IpIntercomCount,
                    x.HasNotTariff
                }).ToListDataResult(loadParams);
        }
    }
}
