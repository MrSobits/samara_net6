namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид услуги
    /// </summary>
    public enum ManagementContractServiceType
    {
        /// <summary>
        /// Коммунальная
        /// </summary>
        [Display("Коммунальная")]
        Communal = 0,

        /// <summary>
        /// Дополнительная
        /// </summary>
        [Display("Дополнительная")]
        Additional = 1,

        /// <summary>
        /// Услуги/Работы по ДУ
        /// </summary>
        [Display("Услуги/Работы по ДУ")]
        Agreement = 2
    }
}
