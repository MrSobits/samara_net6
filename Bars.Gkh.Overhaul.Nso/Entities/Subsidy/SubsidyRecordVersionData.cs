namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using Bars.B4.DataAccess;

    public class SubsidyRecordVersionData : BaseEntity
    {
        /// <summary>
        /// Версия ДПКР
        /// </summary>
        public virtual ProgramVersion Version { get; set; }

        /// <summary>
        /// Ссылка на строку субсидирования
        /// </summary>
        public virtual SubsidyMunicipalityRecord SubsidyRecordUnversioned { get; set; }

        /// <summary>
        /// Потребность в финансировании ДО
        /// </summary>
        public virtual decimal FinanceNeedBefore { get; set; }

        /// <summary>
        /// Потребность в финансировании После
        /// </summary>
        public virtual decimal FinanceNeedAfter { get; set; }

        /// <summary>
        /// Дифицит/Профицит До
        /// </summary>
        public virtual decimal DeficitBefore { get; set; }

        /// <summary>
        /// Дифицит/Профицит После корректировки
        /// </summary>
        public virtual decimal DeficitAfter { get; set; }

        /// <summary>
        /// Рекомендуемый тариф
        /// </summary>
        public virtual decimal RecommendedTarif { get; set; }

        /// <summary>
        /// Собираемость по рекомендуемому тарифу
        /// </summary>
        public virtual decimal RecommendedTarifCollection { get; set; }
    }
}