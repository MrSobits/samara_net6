namespace Bars.Gkh.Gis.Enum
{
    /// <summary>
    /// Секции формата 1.3 загрузки файлов ПУ
    /// </summary>
    public enum PguFileSection
    {
        /// <summary>
        /// Файл информационного описания
        /// </summary>
        InfoDescript,

        /// <summary>
        /// Характеристики жилого фонда
        /// </summary>
        CharacterGilFond,

        /// <summary>
        /// Начисления и расходы по услугам
        /// </summary>
        ChargExpenseServ,

        /// <summary>
        /// Показания счетчиков
        /// </summary>
        Counters,

        /// <summary>
        /// Платежные реквизиты
        /// </summary>
        PaymentDetails,

        /// <summary>
        /// Оплаты
        /// </summary>
        Payment,

        /// <summary>
        /// Информация органов социальной защиты
        /// </summary>
        InfoSocProtection,

        /// <summary>
        /// Дополнительная информация
        /// </summary>
        AdditionalInfo,

        /// <summary>
        /// "Информация об исполнителе
        /// </summary>
        InfoExecutor,

        /// <summary>
        /// Перерасчеты по коммунальным услугам
        /// </summary>
        RevalServ
    }
}