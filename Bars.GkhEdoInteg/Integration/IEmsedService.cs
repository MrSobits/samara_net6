namespace Bars.GkhEdoInteg
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Integration;

    /// <summary>
    /// Работа с сервисами Единой межведомственной системы электронного документооборота
    /// </summary>
    public interface IEmsedService : IIntegrationService
    {
        /// <summary>
        /// Отправка выбранных документов в систему электронного документооборота
        /// </summary>
        /// <param name="baseParams"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool Send(BaseParams baseParams, out string msg);

        /// <summary>
        /// Получение из Эдо сканов обращений граждан
        /// </summary>
        /// <param name="dict">
        /// The dict.
        /// </param>
        /// <param name="message">
        /// </param>
        /// <returns>
        /// </returns>
        bool LoadDocuments(DynamicDictionary dict, out string message);
    }
}
