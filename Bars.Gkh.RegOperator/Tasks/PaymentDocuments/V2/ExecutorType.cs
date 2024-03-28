namespace Bars.Gkh.RegOperator.Tasks.PaymentDocuments.V2
{
    /// <summary>
    /// Тип исполнителя задач
    /// </summary>
    public enum ExecutorType
    {
        /// <summary>
        /// Обработчик физ лиц
        /// </summary>
        Physical,

        /// <summary>
        /// Обработчик юр лиц
        /// </summary>
        Legal,

        /// <summary>
        /// Обработчик юр лиц с одним домом
        /// </summary>
        LegalOneHouse,

        /// <summary>
        /// Обработчик юр лиц по частичному реестру
        /// </summary>
        PartiallyLegal
    }
}