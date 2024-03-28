namespace Bars.Gkh.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип оповещения поставщика ЖКУ
    /// </summary>
    public enum DebtRequestMailNotificationType
    {
        /// <summary>
        /// Уведомление о вновь поступивших запросах без ответа
        /// </summary>
        [Display("Уведомление о вновь поступивших запросах без ответа")]
        RequestWithoutResponse = 1,
        
        /// <summary>
        /// Уведомление о запросах по которым есть неотправленный ответ
        /// </summary>
        [Display("Уведомление о запросах по которым есть неотправленный ответ")]
        RequestWitNotSentResponse = 2
    }
}