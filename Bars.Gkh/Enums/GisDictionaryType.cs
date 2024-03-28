namespace Bars.Gkh.Enums
{
    using B4.Utils;

    /// <summary>
    /// Номера словарей
    /// </summary>
    public enum GisDictionaryType
    {
        /// <summary>
        /// Вид коммунального ресурса
        /// </summary>
        [Display("Вид коммунального ресурса")]
        MunicipalResource = 2,

        /// <summary>
        /// Вид коммунальной услуги
        /// </summary>
        [Display("Вид коммунальной услуги")]
        ServiceType = 3,

        /// <summary>
        /// Полномочие организации
        /// </summary>
        [Display("Полномочие организации")]
        OrganizationRole = 20,

        /// <summary>
        /// Причина закрытия лицевого счета
        /// </summary>
        [Display("Причина закрытия лицевого счета")]
        AccountCloseReason = 22,

        /// <summary>
        /// Характеристика помещения
        /// </summary>
        [Display("Характеристика помещения")]
        ResidentPremiseType = 30,

        /// <summary>
        /// Часовые зоны по Olson
        /// </summary>
        [Display("Часовые зоны по Olson")]
        OlsonTZ = 32,

        /// <summary>
        /// Тип внутренних стен
        /// </summary>
        [Display("Тип внутренних стен")]
        InternalWallType = 49,

        /// <summary>
        /// Вид коммунальной услуги
        /// </summary>
        [Display("Вид коммунальной услуги")]
        MainMunicipalService = 51,

        /// <summary>
        /// Причина расторжения договора
        /// </summary>
        [Display("Причина расторжения договора")]
        ContractTerminationReason = 54,

        /// <summary>
        /// Основание заключения договора
        /// </summary>
        [Display("Основание заключения договора")]
        ContractConclusionReason = 58,

        /// <summary>
        /// Документ, удостоверяющий личность
        /// </summary>
        [Display("Документ, удостоверяющий личность")]
        IdentifierType = 95,

        /// <summary>
        /// Связь вида коммунальной услуги, тарифицируемого ресурса и единиц измерения ставки тарифа
        /// </summary>
        [Display("Связь вида коммунальной услуги, тарифицируемого ресурса и единиц измерения ставки тарифа")]
        UnitByMunicipalService = 236,

        /// <summary>
        /// Тарифицируемый ресурс
        /// </summary>
        [Display("Тарифицируемый ресурс")]
        RatedResource = 239,

        /// <summary>
        /// Показатели качества коммунальных ресурсов
        /// </summary>
        [Display("Показатели качества коммунальных ресурсов")]
        QualityIndicator = 276,

        /// <summary>
        /// Причина аннулирования 
        /// </summary>
        [Display("Причина аннулирования")]
        AnnulmentReason = 330,

        /// <summary>
        /// Интервал поверки
        /// </summary>
        [Display("Интервал поверки")]
        VerificationInterval = 16,

        /// <summary>
        /// Тип прибора учета
        /// </summary>
        [Display("Тип прибора учета")]
        DeviceType = 27,
    }
}