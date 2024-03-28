namespace Bars.Gkh.Export
{
    using System.Collections;
    using System.Linq;

    using B4;
    using B4.Modules.DataExport.Domain;
    using Bars.Gkh.Entities;
    using DomainService;

    /// <summary>
    /// Экспорт Жилого дома, когда нет RegOperator в проекте
    /// </summary>
    public class RealityObjectLiftRegisterDataExport : BaseDataExportService
    {
        /// <summary>
        /// Метод получения Данных для Экспорта
        /// </summary>
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);
            return Container.Resolve<IDomainService<RealityObjectLift>>().GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.PorchNum,
                    x.LiftNum,
                    x.RegNum,
                    x.Capacity,
                    x.StopCount,
                    x.Cost,
                    x.CostEstimate,
                    x.YearEstimate,
                    x.ComissioningDate,
                    x.DecommissioningDate,
                    x.PlanDecommissioningDate,
                    x.ReplacementPeriod,
                    x.YearInstallation,
                    x.YearPlannedReplacement,
                    TypeLift = x.TypeLift != null ? x.TypeLift.Name : string.Empty,
                    ModelLift = x.ModelLift != null ? x.ModelLift.Name : string.Empty,
                    TypeLiftShaft = x.TypeLiftShaft != null ? x.TypeLiftShaft.Name : string.Empty,
                    TypeLiftDriveDoors = x.TypeLiftDriveDoors != null ? x.TypeLiftDriveDoors.Name : string.Empty,
                    TypeLiftMashineRoom = x.TypeLiftMashineRoom != null ? x.TypeLiftMashineRoom.Name : string.Empty,
                    Contragent = x.Contragent != null ? x.Contragent.Name : string.Empty,
                    x.AvailabilityDevices,
                    x.SpeedRise,
                    x.LifeTime,
                    x.YearExploitation,
                    x.NumberOfStoreys,
                    x.DepthLiftShaft,
                    x.WidthLiftShaft,
                    x.HeightLiftShaft,
                    x.OwnerLift,
                    CabinLift = x.CabinLift != null ? x.CabinLift.Name : string.Empty,
                    Info = "Лифт №" + x.RegNum + ", подъезд №" + x.PorchNum,
                    Municipality = x.RealityObject.Municipality != null ? x.RealityObject.Municipality.Name : string.Empty,
                    Settlement = x.RealityObject.MoSettlement != null ? x.RealityObject.MoSettlement.Name : string.Empty,
                    Address = x.RealityObject.Address ?? string.Empty
                })
                .Filter(loadParam, this.Container)
                .Order(loadParam)
                .ToList();
        }
    }
}