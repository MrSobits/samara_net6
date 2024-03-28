namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;

    /// <summary>
    /// ViewHeatingSeason
    /// </summary>
    /*
     * Вьюха для отопительного сезона
     * чтобы получать количественные и Агрегированные показатели:
     * наименование управляющей организации (как в реестре жилых домов)
     * типы договоров (как в реестре жилых домов)
     * наличие документов (акт промывки, паспорт готовности и т.д.)
     */
    public class ViewHeatingSeason : PersistentObject
    {
        /// <summary>
        /// Период отопительного сезона
        /// </summary>
        public virtual HeatSeasonPeriodGji Period { get; set; }

        /// <summary>
        /// Идентификатор периода отопительного сезона
        /// </summary>
        public virtual long? HeatSeasonPeriodId { get; set; }

        /// <summary>
        /// Наименование периода отопительного сезона
        /// </summary>
        public virtual string HeatSeasonPeriodName { get; set; }

        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual string ManOrgName { get; set; }

        /// <summary>
        /// Типы договоров
        /// </summary>
        public virtual string TypeContract { get; set; }

        /// <summary>
        /// Документ "Акт промывки системы отопления"
        /// </summary>
        public virtual bool ActFlushing { get; set; }

        /// <summary>
        /// Документ "Акт опресовки системы отопления"
        /// </summary>
        public virtual bool ActPressing { get; set; }

        /// <summary>
        /// Документ "Акт проверки вентиляционных каналов"
        /// </summary>
        public virtual bool ActVentilation { get; set; }

        /// <summary>
        /// Документ "Акт проверки дымоходов"
        /// </summary>
        public virtual bool ActChimney { get; set; }

        /// <summary>
        /// Документ "Паспорт готовности"
        /// </summary>
        public virtual bool Passport { get; set; }

        /// <summary>
        /// Идентификатор отопительного сезона
        /// </summary>
        public virtual long? HeatingSeasonId { get; set; }

        /// <summary>
        /// Муниицпальное образование
        /// </summary>
        public virtual string Municipality { get; set; }

        /// <summary>
        /// Дата включения тепла
        /// </summary>
        public virtual DateTime? DateHeat { get; set; }

        /// <summary>
        /// Количество подъездов
        /// </summary>
        public virtual int? NumberEntrances { get; set; }

        /// <summary>
        /// Тип дома
        /// </summary>
        public virtual TypeHouse TypeHouse { get; set; }

        /// <summary>
        /// Состояние дома
        /// </summary>
        public virtual ConditionHouse ConditionHouse { get; set; }

        /// <summary>
        /// Отопительная система
        /// </summary>
        public virtual HeatingSystem HeatingSystem { get; set; }

        /// <summary>
        /// Жильцы выселены
        /// </summary>
        public virtual bool ResidentsEvicted { get; set; }

        /// <summary>
        /// Минимальная этажность
        /// </summary>
        public virtual int? Floors { get; set; }

        /// <summary>
        /// Максимальная этажность
        /// </summary>
        public virtual int? MaximumFloors { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Площадь жилого дома
        /// </summary>
        public virtual decimal? AreaMkd { get; set; }

        /// <summary>
        /// Идентификатор муниципального образования
        /// </summary>
        public virtual long? MunicipalityId { get; set; }

        /// <summary>
        /// Дата сдачи в эксплуатацию
        /// </summary>
        public virtual DateTime? DateCommissioning { get; set; }
    }
}