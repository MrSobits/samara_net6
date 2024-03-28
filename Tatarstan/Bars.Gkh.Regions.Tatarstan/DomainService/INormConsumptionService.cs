namespace Bars.Gkh.Regions.Tatarstan.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Regions.Tatarstan.Entities.NormConsumption;

    /// <summary>
    /// Интерфейс для получения нормативов потребления
    /// </summary>
    public interface INormConsumptionService
    {
        /// <summary>
        /// Получить нормативы потребления ГВС
        /// </summary>
        /// <param name="baseParams"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IQueryable<NormConsumptionHotWaterProxy> GetNormConsumptionHotWaterQuery(BaseParams baseParams, out int totalCount);

        /// <summary>
        /// Получить нормативы потребления ХВС
        /// </summary>
        /// <param name="baseParams"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IQueryable<NormConsumptionColdWaterProxy> GetNormConsumptionColdWaterQuery(BaseParams baseParams, out int totalCount);

        /// <summary>
        /// Получить нормативы потребления Подогрев
        /// </summary>
        /// <param name="baseParams"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IQueryable<NormConsumptionHeatingProxy> GetNormConsumptionHeatingQuery(BaseParams baseParams, out int totalCount);

        /// <summary>
        /// Получить нормативы потребления Отопление
        /// </summary>
        /// <param name="baseParams"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IQueryable<NormConsumptionFiringProxy> GetNormConsumptionFiringQuery(BaseParams baseParams, out int totalCount);
    }

    public class NormConsumptionHotWaterProxy : NormConsumptionHotWater
    {
        /// <summary>
        /// Муниципальный район
        /// </summary>
        public string Municipality { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public long ObjectId { get; set; }

        /// <summary>
        /// Количество этажей
        /// </summary>
        public int? FloorNumber { get; set; }

        /// <summary>
        /// Наличие прибора учета
        /// </summary>
        public YesNo MetersInstalled { get; set; }

        /// <summary>
        /// Год постройки
        /// </summary>
        public int? BuildYear { get; set; }

        /// <summary>
        /// Общая площадь дома, кв.м.
        /// </summary>
        public decimal? AreaHouse { get; set; }

        /// <summary>
        /// Общая площадь жилых помещений (квартиры), кв.м
        /// </summary>
        public decimal? AreaLivingRooms { get; set; }

        /// <summary>
        /// Общая площадь нежилых помещений (магазин, аптека и др.) кв.м
        /// </summary>
        public decimal? AreaNotLivingRooms { get; set; }

        /// <summary>
        /// Общая площадь помещений, входящих в состав имущества (лестничные клетки и пр.) кв.м 
        /// </summary>
        public decimal? AreaOtherRooms { get; set; }

        /// <summary>
        /// Вид системы горячего водоснабжения (централизованная, ИТП)
        /// </summary>
        public string TypeSystemHotWater { get; set; }

        /// <summary>
        /// Количество проживающих
        /// </summary>
        public int? ResidentsNumber { get; set; }

        /// <summary>
        /// Износ внутридомовых инженерных сетей
        /// </summary>
        public decimal? DepreciationIntrahouseUtilities { get; set; }

        /// <summary>
        /// Дата проведения капитального ремонта
        /// </summary>
        public DateTime? OverhaulDate { get; set; }

        /// <summary>
        /// В жилых домах квартирного типа с водопроводом, с центральной или местной (выгреб) канализацией 
        /// и централизованным ГВС высотой свыше 12 этажей с централизованным ГВС 
        /// и повышенным требованиям к их благоустройству
        /// </summary>
        public YesNo Gvs12Floor { get; set; }
    }

    public class NormConsumptionColdWaterProxy : NormConsumptionColdWater
    {
        /// <summary>
        /// Муниципальный район
        /// </summary>
        public string Municipality { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public long ObjectId { get; set; }

        /// <summary>
        /// Количество этажей
        /// </summary>
        public int? FloorNumber { get; set; }

        /// <summary>
        /// Наличие прибора учета
        /// </summary>
        public YesNo MetersInstalled { get; set; }

        /// <summary>
        /// Год постройки
        /// </summary>
        public int? BuildYear { get; set; }

        /// <summary>
        /// Общая площадь дома, кв.м.
        /// </summary>
        public decimal? AreaHouse { get; set; }

        /// <summary>
        /// Общая площадь жилых помещений (квартиры), кв.м
        /// </summary>
        public decimal? AreaLivingRooms { get; set; }

        /// <summary>
        /// Общая площадь нежилых помещений (магазин, аптека и др.) кв.м
        /// </summary>
        public decimal? AreaNotLivingRooms { get; set; }

        /// <summary>
        /// Общая площадь помещений, входящих в состав имущества (лестничные клетки и пр.) кв.м 
        /// </summary>
        public decimal? AreaOtherRooms { get; set; }

        /// <summary>
        /// Вид системы горячего водоснабжения (централизованная, ИТП)
        /// </summary>
        public string TypeSystemHotWater { get; set; }

        /// <summary>
        /// Количество проживающих
        /// </summary>
        public int? ResidentsNumber { get; set; }

        /// <summary>
        /// Износ внутридомовых инженерных сетей
        /// </summary>
        public decimal? DepreciationIntrahouseUtilities { get; set; }

        /// <summary>
        /// Дата проведения капитального ремонта
        /// </summary>
        public DateTime? OverhaulDate { get; set; }

        /// <summary>
        /// В жилых домах квартирного типа с водопроводом, с центральной или местной (выгреб) канализацией 
        /// и централизованным ГВС высотой свыше 12 этажей с централизованным ГВС 
        /// и повышенным требованиям к их благоустройству
        /// </summary>
        public YesNo Gvs12Floor { get; set; }
    }

    public class NormConsumptionFiringProxy : NormConsumptionFiring
    {
        /// <summary>
        /// Муниципальный район
        /// </summary>
        public string Municipality { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public long? ObjectId { get; set; }

        /// <summary>
        /// Количество этажей
        /// </summary>
        public int? FloorNumber { get; set; }

        /// <summary>
        /// Год постройки
        /// </summary>
        public int? BuildYear { get; set; }

        /// <summary>
        /// Наличие общедомового прибора учета тепловой энергии (да/нет)
        /// </summary>
        public YesNo GenerealBuildingFiringMeters { get; set; }

        /// <summary>
        /// Общая площадь дома, кв.м.
        /// </summary>
        public decimal? AreaHouse { get; set; }

        /// <summary>
        /// Общая площадь жилых помещений (квартиры), кв.м
        /// </summary>
        public decimal? AreaLivingRooms { get; set; }

        /// <summary>
        /// Общая площадь нежилых помещений (магазин, аптека и др.) кв.м
        /// </summary>
        public decimal? AreaNotLivingRooms { get; set; }

        /// <summary>
        /// Общая площадь помещений, входящих в состав имущества (лестничные клетки и пр.) кв.м 
        /// </summary>
        public decimal? AreaOtherRooms { get; set; }

        /// <summary>
        /// Материал стен  (кирпич, панели, блоки, дерево, смеш и др. материалы)
        /// </summary>
        public string WallMaterial { get; set; }

        /// <summary>
        /// Материал крыши
        /// </summary>
        public string RoofMaterial { get; set; }

        /// <summary>
        /// Износ внутридомовых инженерных сетей %
        /// </summary>
        public decimal? WearIntrahouseUtilites { get; set; }

        /// <summary>
        /// Дата проведения капитального ремонта
        /// </summary>
        public DateTime? OverhaulDate { get; set; }
    }

    public class NormConsumptionHeatingProxy : NormConsumptionHeating
    {
        /// <summary>
        /// Муниципальный район
        /// </summary>
        public string Municipality { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public long ObjectId { get; set; }

        /// <summary>
        /// Количество этажей
        /// </summary>
        public int? FloorNumber { get; set; }

        /// <summary>
        /// Год постройки
        /// </summary>
        public int? BuildYear { get; set; }

        /// <summary>
        /// Наличие общедомового прибора учета тепловой энергии на подогрев
        /// </summary>
        public YesNo GenerealBuildingHeatMeters { get; set; }

        /// <summary>
        /// Общая площадь дома, кв.м.
        /// </summary>
        public decimal? AreaHouse { get; set; }

        /// <summary>
        /// Общая площадь жилых помещений (квартиры), кв.м
        /// </summary>
        public decimal? AreaLivingRooms { get; set; }

        /// <summary>
        /// Общая площадь нежилых помещений (магазин, аптека и др.) кв.м
        /// </summary>
        public decimal? AreaNotLivingRooms { get; set; }

        /// <summary>
        /// Общая площадь помещений, входящих в состав имущества (лестничные клетки и пр.) кв.м 
        /// </summary>
        public decimal? AreaOtherRooms { get; set; }

        /// <summary>
        /// Вид системы горячего водоснабжения (открытая, закрытая)
        /// </summary>
        public string TypeHotWaterSystemStr { get; set; }

        /// <summary>
        /// Дата проведения капитального ремонта
        /// </summary>
        public DateTime? OverhaulDate { get; set; }
    }
}