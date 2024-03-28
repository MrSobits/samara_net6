namespace GisGkhLibrary.Enums
{
    /// <summary>
    /// Номера словарей
    /// </summary>
    public enum DictionaryType
    {
        /// <summary>
        /// Вид коммунального ресурса
        /// </summary>
        MunicipalResource = 2,

        /// <summary>
        /// Вид коммунальной услуги
        /// </summary>
        ServiceType = 3,

        /// <summary>
        /// Полномочие организации
        /// </summary>
        OrganizationRole = 20,

        /// <summary>
        /// Причина закрытия лицевого счета
        /// </summary>
        AccountCloseReason = 22,

        /// <summary>
        /// Характеристика помещения
        /// </summary>
        ResidentPremiseType = 30,

        /// <summary>
        /// Часовые зоны по Olson
        /// </summary>
        OlsonTZ = 32,

        /// <summary>
        /// Тип внутренних стен
        /// </summary>
        InternalWallType = 49,

        /// <summary>
        /// Причина расторжения договора
        /// </summary>
        ContractTerminationReason = 54,

        /// <summary>
        /// Основание заключения договора
        /// </summary>
        ContractConclusionReason = 58,

        /// <summary>
        /// Документ, удостоверяющий личность
        /// </summary>
        IdentifierType = 95,

        /// <summary>
        /// Связь вида коммунальной услуги, тарифицируемого ресурса и единиц измерения ставки тарифа
        /// </summary>
        UnitByMunicipalService = 236,

        /// <summary>
        /// Тарифицируемый ресурс
        /// </summary>
        RatedResource = 239,

        /// <summary>
        /// Показатели качества коммунальных ресурсов
        /// </summary>
        QualityIndicator = 276,

        /// <summary>
        /// Причина аннулирования 
        /// </summary>
        AnnulmentReason = 330,
    }
}
