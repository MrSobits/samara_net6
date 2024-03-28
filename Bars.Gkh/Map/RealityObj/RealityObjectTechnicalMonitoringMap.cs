namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.RealityObj;

    public class RealityObjectTechnicalMonitoringMap : BaseEntityMap<RealityObjectTechnicalMonitoring>
    {
        public RealityObjectTechnicalMonitoringMap() : 
                base("Технический мониторинг", "GKH_OBJ_TECHNICAL_MONITORING")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE").NotNull();
            this.Property(x => x.UsedInExport, "Выводить документ на портал").Column("USED_IN_EXPORT");
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(1000);
            this.Reference(x => x.RealityObject, "Жилой дом").Column("RO_ID").NotNull().Fetch();
            this.Reference(x => x.MonitoringTypeDict, "Тип мониторинга").Column("TYPE_ID").Fetch();
            this.Reference(x => x.File, "Файл").Column("FILE_ID").NotNull();
            this.Property(x => x.TotalBuildingVolume, "Общий строительный объем (куб.м.)").Column("TOTAL_BUILDING_VOL");
            this.Property(x => x.AreaMkd, "Общая площадь (кв.м.)").Column("AREA_MKD");
            this.Property(x => x.AreaLivingNotLivingMkd, "Общая площадь жилых и нежилых помещений (кв.м.)").Column("AREA_LIVING_NOT_LIVING");
            this.Property(x => x.AreaLiving, "В т.ч. жилых всего (кв.м.)").Column("AREA_LIVING");
            this.Property(x => x.AreaNotLiving, "В т.ч. нежилых всего (кв.м.)").Column("AREA_NOT_LIVING");
            this.Property(x => x.AreaNotLivingFunctional, "Общая площадь помещений, входящих в состав общего имущества (кв.м.)").Column("AREA_NOT_LIVING_FUNC");
            this.Property(x => x.Floors, "Количество этажей").Column("FLOORS");
            this.Property(x => x.NumberApartments, "Количество квартир").Column("NUMBER_APARTMENTS");
            this.Reference(x => x.WallMaterial, "Материал стен").Column("WALL_MATERIAL").Fetch();
            this.Property(x => x.PhysicalWear, "Физический износ").Column("PHYSICAL_WEAR");
            this.Property(x => x.TotalWear, "Общий износ").Column("TOTAL_WEAR");
            this.Reference(x => x.CapitalGroup, "Группа капитальности").Column("CAPITAL_GROUP").Fetch();
            this.Property(x => x.WearFoundation, "Износ фундамента").Column("WEAR_FOUNDATION");
            this.Property(x => x.WearWalls, "Износ стен").Column("WEAR_WALLS");
            this.Property(x => x.WearRoof, "Износ крыши").Column("WEAR_ROOF");
            this.Property(x => x.WearInnerSystems, "Износ внутриинженерных систем").Column("WEAR_INNER_SYSTEMS");
            this.Property(x => x.WearHeating, "Износ теплоснабжения").Column("WEAR_HEATING");
            this.Property(x => x.WearWater, "Износ водоснабжения").Column("WEAR_WATER");
            this.Property(x => x.WearWaterCold, "Износ холодного водоснабжения").Column("WEAR_WATER_COLD");
            this.Property(x => x.WearWaterHot, "Износ горячего водоснабжения").Column("WEAR_WATER_HOT");
            this.Property(x => x.WearSewere, "Износ водоотведения").Column("WEAR_SEWERE");
            this.Property(x => x.WearElectric, "Износ электроснабжения").Column("WEAR_ELECTRIC");
            this.Property(x => x.WearLift, "Износ лифта (-ов)").Column("WEAR_LIFT");
            this.Property(x => x.WearGas, "Износ газоснабжения").Column("WEAR_GAS");
        }
    }
}
