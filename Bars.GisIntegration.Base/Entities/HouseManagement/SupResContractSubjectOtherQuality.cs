namespace Bars.GisIntegration.Base.Entities.HouseManagement
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Иной показатель качества коммунального ресурса
    /// </summary>
    public class SupResContractSubjectOtherQuality : BaseRisEntity
    {
        /// <summary>
        /// Предмет договора с поставщиком ресурсов
        /// </summary>
        public virtual SupResContractSubject ContractSubject { get; set; }

        /// <summary>
        /// Наименование показателя
        /// </summary>
        public virtual string IndicatorName { get; set; }

        /// <summary>
        /// Установленное значение показателя качества
        /// </summary>
        public virtual decimal? Number { get; set; }

        /// <summary>
        /// Код ОКЕИ
        /// </summary>
        public virtual string Okei { get; set; }
    }
}
