namespace Bars.Gkh.LogMap
{
    using System.Globalization;

    using Bars.B4.Utils;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Entities.RealityObj;

    /// <summary>
    /// Маппинг сущности "Технический мониторинг" для журнала изменений
    /// </summary>
    public class RealityObjectTechnicalMonitoringLogMap : AuditLogMap<RealityObjectTechnicalMonitoring>
    {
        /// <summary>
        /// Конструктор маппинга
        /// </summary>
        public RealityObjectTechnicalMonitoringLogMap()
        {
            this.Name("Технический мониторинг");

            this.Description(x => x.Description);

            this.MapProperty(x => x.Name, "Name", "Наименование");
            this.MapProperty(x => x.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.UsedInExport, "UsedInExport", "Выводить документ на портал", x => x.GetDisplayName());
            this.MapProperty(x => x.MonitoringTypeDict, "MonitoringTypeDict", "Тип мониторинга", x => x?.Name);
            this.MapProperty(x => x.File, "File", "Файл");
            this.MapProperty(x => x.TotalBuildingVolume, "TotalBuildingVolume", "Общий строительный объем (куб.м.)", x => x?.RoundDecimal(2).ToString());
            this.MapProperty(x => x.AreaMkd, "AreaMkd", "Общая площадь (кв.м.)", x => x?.RoundDecimal(2).ToString());
            this.MapProperty(x => x.AreaLivingNotLivingMkd, "AreaLivingNotLivingMkd", "Общая площадь жилых и нежилых помещений (кв.м.)", x => x?.RoundDecimal(2).ToString());
            this.MapProperty(x => x.AreaLiving, "AreaLiving", "В т.ч. жилых всего (кв.м.)", x => x?.RoundDecimal(2).ToString());
            this.MapProperty(x => x.AreaNotLiving, "AreaNotLiving", "В т.ч. нежилых всего (кв.м.)", x => x?.RoundDecimal(2).ToString());
            this.MapProperty(x => x.AreaNotLivingFunctional, "AreaNotLivingFunctional", "Общая площадь помещений, входящих в состав общего имущества (кв.м.)", x => x?.RoundDecimal(2).ToString());
            this.MapProperty(x => x.Floors, "Floors", "Количество этажей");
            this.MapProperty(x => x.NumberApartments, "NumberApartments", "Количество квартир");
            this.MapProperty(x => x.WallMaterial, "WallMaterial", "Материал стен", x => x?.Name);
            this.MapProperty(x => x.PhysicalWear, "PhysicalWear", "Физический износ", x => x?.RoundDecimal(2).ToString());
            this.MapProperty(x => x.TotalWear, "TotalWear", "Общий износ");
            this.MapProperty(x => x.CapitalGroup, "CapitalGroup", "Группа капитальности", x => x?.Name);
            this.MapProperty(x => x.WearFoundation, "WearFoundation", "Износ фундамента");
            this.MapProperty(x => x.WearWalls, "WearWalls", "Износ стен");
            this.MapProperty(x => x.WearRoof, "WearRoof", "Износ крыши");
            this.MapProperty(x => x.WearInnerSystems, "WearInnerSystems", "Износ внутриинженерных систем");
            this.MapProperty(x => x.WearHeating, "WearHeating", "Износ теплоснабжения");
            this.MapProperty(x => x.WearWater, "WearWater", "Износ водоснабжения");
            this.MapProperty(x => x.WearWaterCold, "WearWaterCold", "Износ холодного водоснабжения");
            this.MapProperty(x => x.WearWaterHot, "WearWaterHot", "Износ горячего водоснабжения");
            this.MapProperty(x => x.WearSewere, "WearSewere", "Износ водоотведения");
            this.MapProperty(x => x.WearElectric, "WearElectric", "Износ электроснабжения");
            this.MapProperty(x => x.WearLift, "WearLift", "Износ лифта (-ов)");
            this.MapProperty(x => x.WearGas, "WearGas", "Износ газоснабжения");
        }

        private YesNo BoolToYesNo(bool value)
        {
            return value ? YesNo.Yes : YesNo.No;
        }
    }
}
