namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Причина перерасчета
    /// </summary>
    public enum RecalcReason
    {
        /// <summary>
        /// Поступление оплаты за закрытый период
        /// </summary>
        [Display("Поступление оплаты за закрытый период")]
        Payment = 0,

        /// <summary>
        /// Перерасчет начислений
        /// </summary>
        [Display("Перерасчет начислений с")]
        RecalcCharge = 1,

        /// <summary>
        /// Изменение параметров расчета пени
        /// </summary>
        [Display("Изменение параметров расчета пени с")]
        ChangePenaltyParametrs = 2,

        /// <summary>
        /// Отмена оплаты за закрытый период
        /// </summary>
        [Display("Отмена оплаты за закрытый период")]
        CancelPayment = 3,

        /// <summary>
        /// Изменение площади помещения
        /// </summary>
        [Display("Изменение площади помещения с")]
        Area = 4,

        /// <summary>
        /// Изменение площади помещения
        /// </summary>
        [Display("Изменение доли собственности с")]
        AreaShare = 5,

        /// <summary>
        /// Изменение даты открытия ЛС
        /// </summary>
        [Display("Изменение даты открытия ЛС с")]
        ChangeOpenDate = 6,

        /// <summary>
        /// Ручной перерасчет
        /// </summary>
        [Display("Ручной перерасчет с")]
        ManuallyRecalc = 7,

        /// <summary>
        /// Изменение даты закрытия ЛС
        /// </summary>
        [Display("Изменение даты закрытия ЛС с")]
        ChangeCloseDate = 8
    }
}