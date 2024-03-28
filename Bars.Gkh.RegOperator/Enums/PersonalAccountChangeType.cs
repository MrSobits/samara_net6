namespace Bars.Gkh.RegOperator.Enums
{
    using System;
    using B4.Utils;

    /// <summary>
    /// Тип операции изменения ЛС
    /// </summary>
    /// <remarks>Используются ФЛАГИ, поэтому каждое значение должно быть степенью 2</remarks>
    [Flags]
    public enum PersonalAccountChangeType
    {
        [Display("Неизвестно")]
        Unknown = 0x0,

        [Display("Слияние")]
        Merge = 0x1,

        [Display("Закрытие")]
        Close = 0x2,

        [Display("Смена абонента")]
        OwnerChange = 0x4,

        [Display("Изменение основной задолженности")]
        SaldoChange = 0x8,

        [Display("Закрытие в связи со слиянием")]
        MergeAndClose = PersonalAccountChangeType.Merge | PersonalAccountChangeType.Close,

        [Display("Изменения сальдо в связи со слиянием")]
        MergeAndSaldoChange = PersonalAccountChangeType.Merge | PersonalAccountChangeType.SaldoChange,

        [Display("Установка и изменение пени")]
        PenaltyChange = 0x16,

        [Display("Отмена начисления пени")]
        PenaltyUndo = 0x32,

        [Display("Отмена начислений и корректировок")]
        ChargeUndo = 0x64,

        [Display("Ручной перерасчет")]
        ManuallyRecalc = 0x128,

        [Display("Зачет средств за ранее выполненные работы")]
        PerformedWorkFundsDistribution = 0x256,

        [Display("Присвоение статуса \"Не активен\"")]
        NonActive = 0x512,

        [Display("Установка и изменение сальдо по базовому тарифу")]
        SaldoBaseChange = 0x1024,

        [Display("Установка и изменение сальдо по тарифу решения")]
        SaldoDecisionChange = 0x2048,

        [Display("Установка и изменение сальдо по пени")]
        SaldoPenaltyChange = 0x4096,

        [Display("Присвоение статуса \"Открыт\"")]
        Open = 0x8192,

        [Display("Перераспределение оплаты")]
        Repayment = 0x16384,

        [Display("Снять блокировку расчета")]
        TurnOffLockFromCalculation = 0x32768,

        [Display("Распределение задолженности")]
        SplitAccount = 0x65536
    }
}