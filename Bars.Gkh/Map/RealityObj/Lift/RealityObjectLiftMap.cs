namespace Bars.Gkh.Map
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Маппинг
    /// </summary>
    public class RealityObjectLiftMap : BaseImportableEntityMap<RealityObjectLift>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public RealityObjectLiftMap() : base("GKH_RO_LIFT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.PorchNum, "PORCH_NUM").Column("PORCH_NUM").Length(100);
            this.Property(x => x.LiftNum, "LIFT_NUM").Column("LIFT_NUM").Length(100);
            this.Property(x => x.FactoryNum, "FACTORY_NUM").Column("FACTORY_NUM").Length(100);
            this.Property(x => x.RegNum, "REG_NUM").Column("REG_NUM").Length(100);
            this.Property(x => x.ReplacementPeriod, "PERIOD_REPLACE").Column("PERIOD_REPLACE").Length(100);
            this.Property(x => x.AvailabilityDevices, "AVAILABILITY_DEVICES").Column("AVAILABILITY_DEVICES");            
            this.Property(x => x.YearInstallation, "YEAR_INSTALATION").Column("YEAR_INSTALATION").DefaultValue(0);
            this.Property(x => x.YearLastUpgradeRepair, "YEAR_LAST_UP_REPAIR").Column("YEAR_LAST_UP_REPAIR").DefaultValue(0);
            this.Property(x => x.YearEstimate, "YEAR_ESTIMATE").Column("YEAR_ESTIMATE").DefaultValue(0);
            this.Property(x => x.YearPlannedReplacement, "YEAR_PLAN_REP").Column("YEAR_PLAN_REP").DefaultValue(0);
            this.Property(x => x.StopCount, "STOP_COUNT").Column("STOP_COUNT").DefaultValue(0);
            this.Property(x => x.Capacity, "CAPACITY").Column("CAPACITY").NotNull().DefaultValue(0);
            this.Property(x => x.Cost, "COST").Column("COST").NotNull().DefaultValue(0);
            this.Property(x => x.CostEstimate, "COST_ESTIMATE").Column("COST_ESTIMATE").NotNull().DefaultValue(0);
            this.Property(x => x.SpeedRise, "SPEED_RISE").Column("SPEED_RISE").NotNull().DefaultValue(0);
            this.Property(x => x.LifeTime, "LIFE_TIME").Column("LIFE_TIME");
            this.Property(x => x.YearExploitation, "YEAR_EXPLOITATION").Column("YEAR_EXPLOITATION").DefaultValue(0);
            this.Property(x => x.NumberOfStoreys, "NUMBER_OF_STOREYS").Column("NUMBER_OF_STOREYS").DefaultValue(0);
            this.Property(x => x.DepthLiftShaft, "DEPTH_LIFT_SHAFT").Column("DEPTH_LIFT_SHAFT").DefaultValue(0);
            this.Property(x => x.WidthLiftShaft, "WIDTH_LIFT_SHAFT").Column("WIDTH_LIFT_SHAFT").DefaultValue(0);
            this.Property(x => x.HeightLiftShaft, "HEIGHT_LIFT_SHAFT").Column("HEIGHT_LIFT_SHAFT").DefaultValue(0);
            this.Property(x => x.DepthCabin, "DEPTH_CABIN").Column("DEPTH_CABIN");
            this.Property(x => x.WidthCabin, "WIDTH_CABIN").Column("WIDTH_CABIN");
            this.Property(x => x.HeightCabin, "HEIGHT_CABIN").Column("HEIGHT_CABIN");
            this.Property(x => x.WidthOpeningCabin, "WIDTH_OPENING_CABIN").Column("WIDTH_OPENING_CABIN");
            this.Property(x => x.OwnerLift, "OWNER_LIFT").Column("OWNER_LIFT").Length(100);
            this.Property(x => x.ComissioningDate, "COMISS_DATE").Column("COMISS_DATE");
            this.Property(x => x.DecommissioningDate, "DECOMISS_DATE").Column("DECOMISS_DATE");
            this.Property(x => x.PlanDecommissioningDate, "PLAN_DECOMISS_DATE").Column("PLAN_DECOMISS_DATE");

            this.Reference(x => x.RealityObject, "RO_ID").Column("RO_ID").NotNull().Fetch();
            this.Reference(x => x.TypeLiftShaft, "LIFT_SHAFT_ID").Column("LIFT_SHAFT_ID").Fetch();
            this.Reference(x => x.TypeLiftMashineRoom, "LIFT_MASH_ROOM_ID").Column("LIFT_MASH_ROOM_ID").Fetch();
            this.Reference(x => x.TypeLiftDriveDoors, "LIFT_DRIVE_DOORS_ID").Column("LIFT_DRIVE_DOORS_ID").Fetch();
            this.Reference(x => x.TypeLift, "LIFT_TYPE_LIFT_ID").Column("LIFT_TYPE_LIFT_ID").Fetch();
            this.Reference(x => x.ModelLift, "LIFT_MODEL_LIFT_ID").Column("LIFT_MODEL_LIFT_ID").Fetch();
            this.Reference(x => x.Contragent, "LIFT_CONTRAGENT_ID").Column("LIFT_CONTRAGENT_ID").Fetch();
            this.Reference(x => x.CabinLift, "LIFT_CABIN_ID").Column("LIFT_CABIN_ID").Fetch();
        }
    }
}