namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Dict
{
    using System.Collections.Generic;

    /// <summary>
    /// Информация о переходе статуса
    /// </summary>
    public class TransferDocStatus: DocStatus
    {
        /// <summary>
        /// Список переходов статусов по заданным типам объектов
        /// </summary>
        public IEnumerable<DocStatus> State { get; set; }
    }
}