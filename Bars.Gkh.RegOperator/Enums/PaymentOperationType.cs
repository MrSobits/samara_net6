namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    //TODO: запилить более кашерный способ разделения операций
    /// <summary>
    /// Тип операции по счету оплат дома
    /// </summary>
    public enum PaymentOperationType
    {
        Default = 0x0,

        /// <summary>
        /// Поступление
        /// </summary>
        [Display("Поступление")]
        Income = 0x1,

        /// <summary>
        /// Поступление займа
        /// </summary>
        [Display("Поступление займа")]
        IncomeLoan = 0x2,

        /// <summary>
        /// Поступление субсидии фонда
        /// </summary>
        [Display("Поступление субсидии фонда")]
        IncomeFundSubsidy = 0x4,

        /// <summary>
        /// Поступление региональной субсидии
        /// </summary>
        [Display("Поступление региональной субсидии")]
        IncomeRegionalSubsidy = 0x5,

        /// <summary>
        /// Поступление стимулирующей субсидии
        /// </summary>
        [Display("Поступление стимулирующей субсидии")]
        IncomeStimulateSubsidy = 0x6,

        /// <summary>
        /// Поступление целевой субсидии
        /// </summary>
        [Display("Поступление целевой субсидии")]
        IncomeGrantInAid = 0x7,

        /// <summary>
        /// Оплата займа
        /// </summary>
        [Display("Оплата займа")]
        ExpenseLoan = 0x8,

        /// <summary>
        /// Оплата по счету
        /// </summary>
        [Display("Оплата по счету")]
        OutcomeAccountPayment = 0x16,

        /// <summary>
        /// Поступление оплаты займа
        /// </summary>
        [Display("Поступление оплаты займа")]
        IncomeLoanPayment = 0x17, // провал с флагами )=

        /// <summary>
        /// Взятие займа у дома
        /// </summary>
        [Display("Взятие займа у дома")]
        OutcomeLoan = 0x18,

        /// <summary>
        /// Поступило по Бюджету МО
        /// </summary>
        [Display("Поступило по Бюджету МО")]
        IncomeBudgetMu = 0x19, 

        /// <summary>
        /// Поступило по Бюджету субъекта
        /// </summary>
        [Display("Поступило по Бюджету субъекта")]
        IncomeBudgetSubject = 0x20, 

        /// <summary>
        /// Поступило из Средств фонда
        /// </summary>
        [Display("Поступило из Средств фонда")]
        IncomeFundResource = 0x21,

        /// <summary>
        /// Поступление оплаты аренды
        /// </summary>
        [Display("Поступление оплаты аренды")]
        RentPaymentIn = 0x22,

        /// <summary>
        /// Ранее накопленные средства
        /// </summary>
        [Display("Ранее накопленные средства")]
        AccumulatedFunds = 0x23,

        /// <summary>
        /// Иной доход МКД
        /// </summary>
        [Display("Иной доход МКД")]
        OtherSources = 0x24,

        /// <summary>
        /// Поступление процентов банка
        /// </summary>
        [Display("Поступление процентов банка")]
        BankPercent = 0x25,

        /// <summary>
        /// Поступление взносов по минимальному тарифу
        /// </summary>
        [Display("Поступление взносов по минимальному тарифу")]
        IncomeByMinTariff = 0x26,

        /// <summary>
        /// Поступление взносов по тарифу решения
        /// </summary>
        [Display("Поступление взносов по тарифу решения")]
        IncomeByDecisionTariff = 0x27,

        /// <summary>
        /// Поступление пени
        /// </summary>
        [Display("Поступление пени")]
        IncomePenalty = 0x28,

        /// <summary>
        /// Поступление оплат за КР ЖЗ
        /// </summary>
        [Display("Поступление оплат за КР ЖЗ")]
        IncomeCr = 0x29,

        /// <summary>
        /// Средства за ранее выполненные работы
        /// </summary>
        [Display("Средства за ранее выполненные работы")]
        PreviousWorkPayment = 0x30,

        /// <summary>
        /// Поступление оплат по "Найму"
        /// </summary>
        [Display("Поступление оплат по \"Найму\"")]
        IncomeRent = 0x31,

        /// <summary>
        /// Расчётно-кассовое обслуживание
        /// </summary>
        [Display("Расчётно-кассовое обслуживание")]
        CashService = 0x32,

        /// <summary>
        /// Плата за открытие счета
        /// </summary>
        [Display("Плата за открытие счета")]
        OpeningAcc = 0x33,

        /// <summary>
        /// Поступление кредита
        /// </summary>
        [Display("Поступление кредита")]
        IncomeCredit = 0x34,

        /// <summary>
        /// Погашение кредита
        /// </summary>
        [Display("Погашение кредита")]
        CreditPayment = 0x35,

        /// <summary>
        /// Погашение процентов по кредиту
        /// </summary>
        [Display("Погашение процентов по кредиту")]
        CreditPercentPayment = 0x36,

        /// <summary>
        /// Поступление социальной поддержки
        /// </summary>
        [Display("Поступление социальной поддержки")]
        IncomeSocialSupport = 0x37,

        /// <summary>
        /// Расход на получение гарантий
        /// </summary>
        [Display("Расход на получение гарантий")]
        GuaranteesObtainPayment = 0x38,

        /// <summary>
        /// Поручительства по кредитам
        /// </summary>
        [Display("Поручительства по кредитам")]
        GuaranteesForCredit = 0x39,

        /// <summary>
        /// Отмена платежа
        /// </summary>
        [Display("Отмена платежа")]
        UndoPayment = 0x40,

        /// <summary>
        /// Отмена оплаты (операция над л/с)
        /// </summary>
        [Display("Отмена оплаты")]
        CancelPayment = 0x41
    }
}