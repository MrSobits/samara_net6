namespace Bars.GkhGji.Entities
{
    using B4.DataAccess;
    using B4.Modules.States;
    
    using Enums;
    using Gkh.Enums;

    public class ViewHeatSeasonDoc : PersistentObject
    {
        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Тип документа подготовки к отопительному сезону
        /// </summary>
        public virtual HeatSeasonDocType TypeDocument { get; set; }

        /// <summary>
        /// Идентификатор периода отопительного сезона
        /// </summary>
        public virtual long? PeriodId { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Тип дома
        /// </summary>
        public virtual TypeHouse TypeHouse { get; set; }

        /// <summary>
        /// Система отопления
        /// </summary>
        public virtual HeatingSystem HeatingSystem { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual string MunicipalityName { get; set; }

        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual string ManOrgName { get; set; }

        /// <summary>
        /// Состояние дома
        /// </summary>
        public virtual ConditionHouse ConditionHouse { get; set; }
    }
}