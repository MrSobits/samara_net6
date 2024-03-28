namespace Bars.Gkh.RegOperator.Enums
{
    /// <summary>
    /// Тип события для перерасчета
    /// </summary>
    public enum RecalcEventType
    {
        /// <summary>
        /// Изменение даты открытия ЛС
        /// </summary>
        ChangeOpenDate = 10,

        /// <summary>
        /// Изменение даты закрытия ЛС
        /// </summary>
        ChangeCloseDate = 20,

        /// <summary>
        /// Смена абонента
        /// </summary>
        ChangeOwner = 30,

        /// <summary>
        /// Поступление оплаты
        /// </summary>
        Payment = 40,

        /// <summary>
        /// Изменение настроек пени
        /// </summary>
        ChangePenaltyParam = 50,

        /// <summary>
        /// Отмена оплаты
        /// </summary>
        CancelPayment = 60
    }
}