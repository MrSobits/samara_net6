namespace Bars.Gkh.Overhaul.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;

    /// <summary>
    /// Представление <see cref="OvrhlRealityObjectLift"/>
    /// </summary>
    public class RealityObjectLiftViewModel : BaseViewModel<OvrhlRealityObjectLift>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<OvrhlRealityObjectLift> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var roId = loadParams.Filter.GetAs("ro_id", 0L);
            if (roId == 0)
            {
                roId = baseParams.Params.GetAs("ro_id", 0L);
            }

            var data = domain.GetAll()
                .Where(x => x.RealityObject.Id == roId)
                .Select(
                    x => new
                    {
                        x.Id,
                        x.PorchNum,
                        x.LiftNum,
                        x.RegNum,
                        x.Capacity,
                        x.StopCount,
                        x.Cost,
                        x.ComissioningDate,
                        x.DecommissioningDate,
                        x.PlanDecommissioningDate,
                        x.CostEstimate,
                        x.YearEstimate,
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
                        x.RealityObjectStructuralElement
                    })
                .Filter(loadParams, this.Container);

            var count = data.Count();
            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), count);
        }
    }
}