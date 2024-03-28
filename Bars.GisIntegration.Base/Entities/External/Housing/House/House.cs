namespace Bars.GisIntegration.Base.Entities.External.Housing.House
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.GisIntegration.Base.Entities.External.Administration.System;
    using Bars.GisIntegration.Base.Entities.External.Dict.Common;
    using Bars.GisIntegration.Base.Entities.External.Dict.House;
    using Bars.GisIntegration.Base.Entities.External.Mo;

    using HouseState = Dict.House.HouseState;
    using TimeZone = Bars.GisIntegration.Base.Entities.External.Dict.House.TimeZone;

    /// <summary>
    /// Дом
    /// </summary>
    public class House : BaseEntity
    {
        /// <summary>
        /// Поставщик информации
        /// </summary>
        public virtual DataSupplier DataSupplier { get; set; }

        /// <summary>
        /// Условный номер ЕГРП
        /// </summary>
        public virtual string EgrpNumber { get; set; }

        /// <summary>
        /// Уникальный идентификатор дома в ГИС ЖКХ
        /// </summary>
        public virtual string GisGuid { get; set; }
        /// <summary>
        /// Кадастровый номер
        /// </summary>
        public virtual string CadastrNumber { get; set; }
        /// <summary>
        /// Связь с ГКН отсутствует
        /// </summary>
        public virtual bool IsGknNone { get; set; }
        /// <summary>
        /// Глобальный идентификатор дома по ФИАС
        /// </summary>
        public virtual string HouseFiasGuid { get; set; }
        /// <summary>
        /// Адрес дома
        /// </summary>
        public virtual FiasAddress FiasAddress { get; set; }
        /// <summary>
        /// Муниципальная организация
        /// </summary>
        public virtual MoTerritory MoTerritory { get; set; }
        /// <summary>
        /// Временная зона
        /// </summary>
        public virtual TimeZone TimeZone { get; set; }
        /// <summary>
        /// Общая площадь
        /// </summary>
        public virtual decimal TotalSquare { get; set; }
        /// <summary>
        /// Площадь застройки
        /// </summary>
        public virtual decimal BuildSquare { get; set; }
        /// <summary>
        /// Жилая площадь
        /// </summary>
        public virtual decimal LiveSquare { get; set; }
        /// <summary>
        /// Не жилая площадь
        /// </summary>
        public virtual decimal NonliveSquare { get; set; }
        /// <summary>
        /// Площадь мест общего пользования
        /// </summary>
        public virtual decimal MopSquare { get; set; }
        /// <summary>
        /// Серия проекта здания
        /// </summary>
        public virtual string ProjectSeries { get; set; }
        /// <summary>
        /// Тип проекта здания
        /// </summary>
        public virtual BuildProjectType BuildProjectType { get; set; }
        /// <summary>
        /// Тип внутренних стен
        /// </summary>
        public virtual InnerWallType InnerWallType { get; set; }
        /// <summary>
        /// Год постройки
        /// </summary>
        public virtual DateTime? BuildYear { get; set; }
        /// <summary>
        /// Год ввода в эксплуатацию
        /// </summary>
        public virtual DateTime? StartUpYear { get; set; }
        /// <summary>
        /// Год последнего кап ремонта
        /// </summary>
        public virtual DateTime? LastCrYear { get; set; }
        /// <summary>
        /// Год проведения реконструкции
        /// </summary>
        public virtual DateTime? RestoreYear { get; set; }
        /// <summary>
        /// Тип дома - идентификатор
        /// </summary>
        public virtual long HouseTypeId { get; set; }
        /// <summary>
        /// Тип дома
        /// </summary>
        public virtual HouseType HouseType { get; set; }
        /// <summary>
        /// Состояние дома
        /// </summary>
        public virtual HouseState HouseState { get; set; }
        /// <summary>
        /// Наличие у дома объекта культурного наследия
        /// </summary>
        public virtual bool IsCultureHeritage { get; set; }
        /// <summary>
        /// Общий износ здания
        /// </summary>
        public virtual decimal Wearout { get; set; }
        /// <summary>
        /// Дата, на которую установлен износ здания
        /// </summary>
        public virtual DateTime? WearoutDate { get; set; }
        /// <summary>
        /// Количество этажей
        /// </summary>
        public virtual int FloorCount { get; set; }
        /// <summary>
        /// Максимальная этажность
        /// </summary>
        public virtual int MaxFloor { get; set; }
        /// <summary>
        /// Минимальная этажность
        /// </summary>
        public virtual int MinFloor { get; set; }
        /// <summary>
        /// Количество подземных этажей
        /// </summary>
        public virtual int UndergroudFloorCount { get; set; }
        /// <summary>
        /// Класс энергетической эффективности
        /// </summary>
        public virtual EnergyEfficiencyClass EnergyEfficiencyClass { get; set; }
        /// <summary>
        /// Дата проведения энергетического обследования
        /// </summary>
        public virtual DateTime? EnergySurveyDate { get; set; }
        /// <summary>
        /// Способ формирования фонда кап ремонта
        /// </summary>
        public virtual CrFormingType CrFormingType { get; set; }
        /// <summary>
        /// Тип управления
        /// </summary>
        public virtual ManagMode ManagMode { get; set; }
        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagementOrganization ManagementOrganization { get; set; }
        /// <summary>
        /// Отапливаемая площадь
        /// </summary>
        public virtual decimal HeatSquare { get; set; }
        /// <summary>
        /// Стадия жизненного цикла
        /// </summary>
        public virtual string LifeCycleStage { get; set; }
        /// <summary>
        /// Пользователь 
        /// </summary>
        public virtual int ChangedBy { get; set; }
        /// <summary>
        /// Дата изменения 
        /// </summary>
        public virtual DateTime ChangedOn { get; set; }

    }
}
