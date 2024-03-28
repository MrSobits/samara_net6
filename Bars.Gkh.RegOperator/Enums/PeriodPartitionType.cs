namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Причина разделения периода
    /// </summary>
    public enum PeriodPartitionType
    {
        /// <summary>
        /// Поступление оплаты
        /// </summary>
        [Display("Поступление оплаты за {0} на сумму {1} рублей{2}")]
        Payment = 10,

        /// <summary>
        /// Возврат или отмена
        /// </summary>
        [Display("Отмена оплаты/возврат долга {0} на сумму {1} рублей")]
        Refund = 20,

        /// <summary>
        /// Изменение ставки рефинансирования
        /// </summary>
        [Display("Изменение ставки рефинансирования с {0}")]
        PercentageChange = 30,

        /// <summary>
        /// Изменение допустимого количества дней просрочки
        /// </summary>
        [Display("Изменение срока оплаты с {0} на {1} дней")]
        DebtDaysChange = 40,

        /// <summary>
        /// Корректировка оплаты
        /// </summary>
        [Display("Корректировка оплаты за {0} на сумму {1} рублей")]
        PaymentCorrection = 50,

        /// <summary>
        /// Изменение допустимого количества дней просрочки
        /// </summary>
        [Display("Зачет средств за выполненные работы за {0} на сумму {1} рублей")]
        PerfWork = 60,

        /// <summary>
        /// Пустой
        /// </summary>
        Empty = 70,

        /// <summary>
        /// Закрытие лс
        /// </summary>
        [Display("Изменение даты закрытия ЛС")]
        CloseAccount = 80,

        /// <summary>
        /// Вступление в силу договора реструктуризации
        /// </summary>
        [Display("Вступление в силу договора реструктуризации")]
        Restruct = 90,

        /// <summary>
        /// Вступление в силу договора реструктуризации
        /// </summary>
        [Display("Просрочка оплаты по графику реструктуризации")]
        LatePymentRestruct = 100
    }
}