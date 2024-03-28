namespace Bars.Gkh.Overhaul.Tat.Entities
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
        /// Необходимая сумма на КР в этой году
        /// </summary>
        public virtual decimal NeedFinance { get; set; }

        /// <summary>
        /// Остаток либо положительный либо отрицательный
        /// </summary>
        public virtual decimal Deficit { get; set; }

        /// <summary>
        /// Скорректированная потребность финансирования
        /// </summary>
        public virtual decimal CorrNeedFinance { get; set; }

        /// <summary>
        /// Скорректированный остаток
        /// </summary>
        public virtual decimal CorrDeficit { get; set; }
    }
}