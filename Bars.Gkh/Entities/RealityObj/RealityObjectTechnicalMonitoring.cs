namespace Bars.Gkh.Entities.RealityObj
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;

    public class RealityObjectTechnicalMonitoring : BaseEntity
    {
        /// <summary>
        /// Дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        public virtual MonitoringTypeDict MonitoringTypeDict { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime DocumentDate { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Выводить документ на портал
        /// </summary>
        public virtual YesNo UsedInExport { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Общий строительный объем (куб.м.)
        /// </summary>
        public virtual decimal? TotalBuildingVolume { get; set; }

        /// <summary>
        /// Общая площадь (кв.м.)
        /// </summary>
        public virtual decimal? AreaMkd { get; set; }

        /// <summary>
        /// Общая площадь жилых и нежилых помещений (кв.м.)
        /// </summary>
        public virtual decimal? AreaLivingNotLivingMkd { get; set; }

        /// <summary>
        /// Площадь жилых помещений (кв.м.)
        /// </summary>
        public virtual decimal? AreaLiving { get; set; }

        /// <summary>
        /// Площадь нежилых помещений (кв.м.)
        /// </summary>
        public virtual decimal? AreaNotLiving { get; set; }

        /// <summary>
        /// Общая площадь помещений, входящих в состав общего имущества (кв.м.)
        /// </summary>
        public virtual decimal? AreaNotLivingFunctional { get; set; }

        /// <summary>
        /// Количество этажей
        /// </summary>
        public virtual int? Floors { get; set; }

        /// <summary>
        /// Количество квартир
        /// </summary>
        public virtual int? NumberApartments { get; set; }

        /// <summary>
        /// Материал стен
        /// </summary>
        public virtual WallMaterial WallMaterial { get; set; }

        /// <summary>
        /// Физический износ
        /// </summary>
        public virtual decimal? PhysicalWear { get; set; }

        /// <summary>
        /// Общий износ
        /// </summary>
        public virtual int? TotalWear { get; set; }

        /// <summary>
        /// Группа капитальности
        /// </summary>
        public virtual CapitalGroup CapitalGroup { get; set; }

        /// <summary>
        /// Износ
        /// </summary>
        public virtual int? WearAll { get; set; }

        /// <summary>
        /// Износ фундамента
        /// </summary>
        public virtual int? WearFoundation { get; set; }

        /// <summary>
        /// Износ стен
        /// </summary>
        public virtual int? WearWalls { get; set; }

        /// <summary>
        /// Износ крыши
        /// </summary>
        public virtual int? WearRoof { get; set; }

        /// <summary>
        /// Износ внутриинженерных систем
        /// </summary>
        public virtual int? WearInnerSystems { get; set; }

        /// <summary>
        /// Износ теплоснабжения
        /// </summary>
        public virtual int? WearHeating { get; set; }

        /// <summary>
        /// Износ водоснабжения
        /// </summary>
        public virtual int? WearWater { get; set; }

        /// <summary>
        /// Износ холодного водоснабжения
        /// </summary>
        public virtual int? WearWaterCold { get; set; }

        /// <summary>
        /// Износ горячего водоснабжения
        /// </summary>
        public virtual int? WearWaterHot { get; set; }

        /// <summary>
        /// Износ водоотведения
        /// </summary>
        public virtual int? WearSewere { get; set; }

        /// <summary>
        /// Износ электроснабжения
        /// </summary>
        public virtual int? WearElectric { get; set; }

        /// <summary>
        /// Износ лифта (-ов)
        /// </summary>
        public virtual int? WearLift { get; set; }

        /// <summary>
        /// Износ газоснабжения
        /// </summary>
        public virtual int? WearGas { get; set; }

    }
}