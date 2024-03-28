using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Sobits.GisGkh.Enums;
using System;

namespace Sobits.GisGkh.Entities
{
    /// <summary>
    /// Запросы в ГИС ЖКХ
    /// </summary>
    public class GisGkhRequests : BaseEntity
    {
        /// <summary>
        /// GUID сообщения ГИС ЖКХ
        /// </summary>
        public virtual string MessageGUID { get; set; }

        /// <summary>
        /// GUID сообщения отправителя
        /// </summary>
        public virtual string RequesterMessageGUID { get; set; }

        /// <summary>
        /// Вид запроса
        /// </summary>
        public virtual GisGkhTypeRequest TypeRequest { get; set; }

        /// <summary>
        /// Текущее состояние запроса
        /// </summary>
        public virtual GisGkhRequestState RequestState { get; set; }

        /// <summary>
        /// Ответ от сервера
        /// </summary>
        public virtual string Answer { get; set; }

        /// <summary>
        /// Инициатор запроса
        /// </summary>
        public virtual Operator Operator { get; set; }

        /// <summary>
        /// Номер справочника для обновления
        /// </summary>
        public virtual string DictionaryNumber { get; set; }

        /// <summary>
        /// Лог файл
        /// </summary>
        public virtual FileInfo LogFile { get; set; }
    }
}
