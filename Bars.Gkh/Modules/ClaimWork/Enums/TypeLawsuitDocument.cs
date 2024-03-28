namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип документа
    /// </summary>
    public enum TypeLawsuitDocument
    {
        /// <summary>
        /// Постановление о возбуждении исполнительного производства
        /// </summary>
        [Display("Постановление о возбуждении исполнительного производства")]
        InitiateEnforcementProceedingsDecision = 10,

        /// <summary>
        /// Постановление об отказе в возбуждении исполнительного производства
        /// </summary>
        [Display("Постановление об отказе в возбуждении исполнительного производства")]
        DecisionRefusalInitiateEnforcementProceedings = 20,

        /// <summary>
        /// Постановление об отложении исполнительного производства
        /// </summary>
        [Display("Постановление об отложении исполнительного производства")]
        DecisionPostponeEnforcementProceedings = 30,

        /// <summary>
        /// Постановление об прекращении исполнительного производства
        /// </summary>
        [Display("Постановление об прекращении исполнительного производства")]
        DecisionTerminationEnforcementProceedings = 40,

        /// <summary>
        /// Постановление об прекращении исполнительного производства
        /// </summary>
        [Display("Постановление об окончании исполнительного производства")]
        DecisionEndingOfEnforcementProceedings = 50
    }
}