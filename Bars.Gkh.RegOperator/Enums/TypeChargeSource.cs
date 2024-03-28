namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип источника поступления начислений.
    /// <returns><see cref="DescriptionAttribute"/> используется для получения данных для сохранения истории</returns>
    /// </summary>
    public enum TypeChargeSource
    {
        /// <summary>
        /// Начисления
        /// </summary>
        [Display("Начисления")]
        [Description("Начисления")]
        Charge = 0,
        
        /// <summary>
        /// Отмена начислений и пени
        /// </summary>
        [Display("Отмена начислений и пени")]
        [Description("Отмена начислений и пени")]
        CancelCharge = 10,

        /// <summary>
        /// Перенос долга при слиянии
        /// </summary>
        [Display("Перенос долга при слиянии")]
        [Description("Перенос долга при слиянии")]
        MergeCharge = 20,

        /// <summary>
        /// Импорт начислений в закрытый период
        /// </summary>
        [Display("Импорт начислений в закрытый период")]
        [Description("Импорт начислений в закрытый период")]
        ImportsIntoClosedPeriod = 30,

        /// <summary>
        /// Массовое изменение сальдо лицевых через импорт
        /// </summary>}
        [Display("Массовое изменение сальдо лицевых счетов через импорт")]
        [Description("Корректировка сальдо через импорт")]
        ImportSaldoChangeMass = 40,

        /// <summary>
        /// Изменение основной задолженности
        /// </summary>
        [Display("Изменение основной задолженности")]
        [Description("Изменение основной задолженности")]
        BalanceChange = 50,

        /// <summary>
        /// Изменение сальдо по пени
        /// </summary>
        [Display("Изменение пени счета")]
        PenaltyChange = 60,

        /// <summary>
        /// Массовое изменение сальдо лицевых
        /// </summary>}
        [Display("Массовое изменение сальдо лицевых счетов")]
        [Description("Ручная корректировка сальдо")]
        SaldoChangeMass = 70,

        /// <summary>
        /// Зачет средств за ранее выполненные работы
        /// </summary>}
        [Display("Зачет средств за ранее выполненные работы")]
        [Description("Зачет средств за ранее выполненные работы")]
        PerformedWorkCharge = 80,

        /// <summary>
        /// Отмена начислений и пени из импорта
        /// </summary>
        [Display("Отмена начислений и пени  из импорта")]
        [Description("Отмена начислений и пени из импорта")]
        ImportCancelCharge = 90,

        /// <summary>
        /// Разделение лицевого счета
        /// </summary>
        [Display("Разделение лицевого счета")]
        [Description("Разделение лицевого счета")]
        SplitAccount = 100
    }
}