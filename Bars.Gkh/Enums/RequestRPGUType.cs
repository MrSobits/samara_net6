namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип запроса
    /// </summary>
    public enum RequestRPGUType
    {
        /// <summary>Создано</summary>
        [Display("Запрос дополнительной информаци")]
        InformationRequest = 10,

        /// <summary>В очереди</summary>
        [Display("Запрос оплаты пошлины")]
        DutyRequest = 20,

        /// <summary>В очереди</summary>
        [Display("Не задано")]
        NotSet = 30,


    }
}