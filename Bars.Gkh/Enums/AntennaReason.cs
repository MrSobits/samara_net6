namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Причины отсутствия СКПТ
    /// </summary>
    public enum AntennaReason
    {
        /// <summary>
        /// Не выбран
        /// </summary>
        [Display("-")]
        None = 0,

        /// <summary>
        /// Отказ собственников
        /// </summary>
        [Display("Отказ собственников")]
        Refuse = 10,

        /// <summary>
        /// Отсутствие проектной документации
        /// </summary>
        [Display("Отсутствие проектной документации")]
        Lack = 20,

        /// <summary>
        /// Дециметровый
        /// </summary>
        [Display("Отсутствие СКПТ в связи с капитальным ремонтом крыши")]
        Cr = 30
    }
}