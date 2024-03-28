namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities.NormConsumption;

    public class NormConsumptionColdWaterMap : JoinedSubClassMap<NormConsumptionColdWater>
    {
        public NormConsumptionColdWaterMap()
            : base("Норматив потребления холодной воды", "GKH_TAT_NORM_CONS_COLD_WATTER")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.IsIpuNotLivingPermises, "Наличие отдельного ипу у нежилых помещений").Column("IS_IPU_NOT_LIV_PERMISES").NotNull();
            this.Property(x => x.AreaIpuNotLivingPermises, "Площадь нежилых помещений при " +
                "наличии отдельного ипу у нежилых помещений").Column("AREA_IPU_NOT_LIV_PERMISES");
            this.Property(x => x.VolumeColdWaterNotLivingIsIpu, "Объем ГВС по нежилым помещениям " +
                "при наличии ипу").Column("VOL_COLD_WATER_NOT_LIV_ISIPU");
            this.Property(x => x.VolumeWaterOpuOnPeriod, "Для домов с ОПУ Объем воды по общедомовому " +
                "прибору за отопительный период").Column("VOL_WATER_OPU_ON_PERIOD");
            this.Property(x => x.HeatingPeriod, "Отопительный период, дни").Column("HEATING_PERIOD");
            this.Property(x => x.IsBath1200, "МКД и жилые дома с централ. ХВС и ГВС, водоотведением, оборудованные унитазами, раковинами," +
                " мойками, ваннами сидячими длиной 1200 мм с душем").Column("IS_BATH_1200").NotNull();
            this.Property(x => x.IsBath1500With1550, "МКД и жилые дома с централ. ХВС и ГВС, водоотведением, оборудованные унитазами," +
                " раковинами, мойками, ваннами длиной 1500 - 1550 мм с душем").Column("IS_BATH_1500_WITH_1550").NotNull();
            this.Property(x => x.IsBath1650With1700, "МКД и жилые дома с централ. ХВС и ГВС, водоотведением, оборудованные унитазами, " +
                "раковинами, мойками, ваннами длиной 1650 - 1700 мм с душем").Column("IS_BATH_1650_WITH_1700").NotNull();
            this.Property(x => x.IsBathNotShower, "МКД и жилые дома с централ. ХВС и ГВС, водоотведением, оборудованные унитазами, " +
                "раковинами, мойками, ваннами без душа").Column("IS_BATH_NOT_SHOWER").NotNull();
            this.Property(x => x.IsShower, "МКД и жилые дома с централ. ХВС и ГВС, водоотведением, " +
                "оборудованные унитазами, раковинами, мойками, душем").Column("IS_SHOWER").NotNull();
            this.Property(x => x.HvsIsBath1200, "МКД и жилые дома с централ. ХВС, водонагревателями, водоотведением, оборудованные унитазами, " +
                "раковинами, мойками, душами и ваннами сидячими длиной 1200 мм с душем").Column("HVS_IS_BATH_1200").NotNull();
            this.Property(x => x.HvsIsBath1500With1550, "МКД и жилые дома с централ. ХВС, водонагревателями, водоотведением, оборудованные унитазами, " +
                "раковинами, мойками, душами и ваннами длиной 1500 - 1550 мм с душем").Column("HVS_IS_BATH_1500_WITH_1550").NotNull();
            this.Property(x => x.HvsIsBathNotShower, "МКД и жилые дома с централ. ХВС, водонагревателями, водоотведением, " +
                "оборудованные унитазами, раковинами, мойками, душами и ваннами без душа").Column("HVS_IS_BATH_NOT_SHOWER").NotNull();
            this.Property(x => x.HvsIsShower, "МКД и жилые дома с централ. ХВС, водонагревателями, водоотведением, " +
                "оборудованные унитазами, раковинами, мойками, душами").Column("HVS_IS_SHOWER").NotNull();
            this.Property(x => x.IsNotBoiler, "МКД и жилые дома без водонагревателей с водопроводом и канализацией, " +
                "оборудованные раковинами, мойками и унитазами").Column("IS_NOT_BOILER").NotNull();
            this.Property(x => x.HvsIsNotBoiler, "МКД и жилые дома без водонагревателей с централ. ХВС и водоотведением, " +
                "оборудованные раковинами и мойками").Column("HVS_IS_NOT_BOILER").NotNull();
            this.Property(x => x.IsHvsBathIsNotCentralSewage, "МКД и жилые дома с централ. ХВС, без централизованного водоотведения, " +
                "оборудованные умывальниками, мойками, унитазами, ваннами, душами ").Column("IS_HVS_BATH_IS_NOT_CNTRL_SEW").NotNull();
            this.Property(x => x.IsHvsIsNotCentralSewage, "МКД и жилые дома с централ. ХВС, без централизованного водоотведения, " +
                "оборудованные умывальниками, мойками, унитазами").Column("IS_HVS_IS_NOT_CNTRL_SEW").NotNull();
            this.Property(x => x.IsStandpipes, "МКД и жилые дома с водоразборной колонкой").Column("IS_STANDPIPES").NotNull();
            this.Property(x => x.IsHostelNoShower, "Дома, использующиеся в качестве общежитий без душевых").Column("IS_HOSTEL_NOSHOWER").NotNull();
            this.Property(x => x.IsHostelSharedShower, "Дома, использующиеся в качестве общежитий с общими душевыми")
                .Column("IS_HOSTEL_SHARED_SHOWER").NotNull();
            this.Property(x => x.IsHostelShowerAllLivPermises, "Дома, использующиеся в качестве общежитий с душами при всех жилых комнатах")
                .Column("IS_HOSTEL_SHOWER_LIV_PER").NotNull();
            this.Property(x => x.ShowerInHostelInSection, "Дома, использующиеся в качестве общежитий с общими кухнями " +
                "и блоками душевых на этажах при жилых комнатах в каждой секции здания").Column("SHOWER_IN_HOSTEL_IN_SECT").NotNull();
        }
    }
}