namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities.NormConsumption;

    public class NormConsumptionHotWaterMap : JoinedSubClassMap<NormConsumptionHotWater>
    {
        public NormConsumptionHotWaterMap()
            : base("Норматив потребления горячей воды", "GKH_TAT_NORM_CONS_HOT_WATTER")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.IsIpuNotLivingPermises, "Наличие отдельного ипу у нежилых помещений").Column("IS_IPU_NOT_LIV_PERMISES").NotNull();
            this.Property(x => x.AreaIpuNotLivingPermises, "Площадь нежилых помещений при " +
                "наличии отдельного ипу у нежилых помещений").Column("AREA_IPU_NOT_LIV_PERMISES");
            this.Property(x => x.VolumeHotWaterNotLivingIsIpu, "Объем ГВС по нежилым помещениям " +
                "при наличии ипу").Column("VOL_HOT_WATER_NOT_LIV_ISIPU");
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
            this.Property(x => x.SharedShowerInHostel, "Дома, использующиеся в качестве общежитий с общими душевыми")
                .Column("SHARED_SHOWER_INHOSTEL").NotNull();
            this.Property(x => x.IsHostelShowerAllLivPermises, "Дома, использующиеся в качестве общежитий " +
                "с душами при всех жилых комнатах").Column("SHARED_SHOWER_HSTL_ALL_LIV_PRM").NotNull();
            this.Property(x => x.ShowerInHostelInSection, "Дома, использующиеся в качестве общежитий с общими кухнями " +
                "и блоками душевых на этажах при жилых комнатах в каждой секции здания").Column("SHOWER_IN_HOSTEL_IN_SECT").NotNull();
        }
    }
}