namespace Bars.Gkh.ViewModel.RealityObject
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.RealityObj;
    using Bars.Gkh.Utils;

    public class RealityObjectTechnicalMonitoringViewModel : BaseViewModel<RealityObjectTechnicalMonitoring>
    {
        public override IDataResult List(IDomainService<RealityObjectTechnicalMonitoring> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var roId = loadParams.Filter.GetAsId("realityObjectId");

            return domain.GetAll()
                .Where(x => x.RealityObject.Id == roId) 
                .Select(x => new
                {
                    x.Id,
                    x.RealityObject,
                    MonitoringTypeDict = x.MonitoringTypeDict!= null? x.MonitoringTypeDict.Name:"",
                    x.Name,
                    x.Description,
                    x.File,
                    x.DocumentDate,
                    x.UsedInExport,
                    x.TotalBuildingVolume,
                    x.AreaMkd,
                    x.AreaLivingNotLivingMkd,
                    x.AreaLiving,
                    x.AreaNotLiving,
                    x.AreaNotLivingFunctional,
                    x.CapitalGroup,
                    x.Floors,
                    x.NumberApartments,
                    x.PhysicalWear,
                    x.TotalWear,
                    x.WearFoundation,
                    x.WearWalls,
                    x.WearRoof,
                    x.WearInnerSystems,
                    x.WearHeating,
                    x.WearWater,
                    x.WearWaterCold,
                    x.WearWaterHot,
                    x.WearSewere,
                    x.WearElectric,
                    x.WearLift,
                    x.WearGas
                })
                .ToListDataResult(loadParams, this.Container);
        }
    }
}