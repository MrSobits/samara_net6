using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.Gkh.Enums.Administration.EmailMessage;
using System;

namespace Bars.Gkh.Entities.Administration
{
    /// <summary>
    /// Отправленное письмо
    /// </summary>
    public class EmailMessage : BaseEntity
    {
        /// <summary>
        /// Тип отправляемого сообщения
        /// </summary>
        public virtual EmailMessageType EmailMessageType { get; set; }

        /// <summary>
        /// Получатель письма
        /// </summary>
        public virtual Contragent RecipientContragent { get; set; }

        /// <summary>
        /// Адрес электронной почты получателя
        /// </summary>
        public virtual string EmailAddress { get; set; }

        /// <summary>
        /// Дополнительные сведения
        /// </summary>
        public virtual string AdditionalInfo { get; set; }

        /// <summary>
        /// Время отправки
        /// </summary>
        public virtual DateTime SendingTime { get; set; }

        /// <summary>
        /// Cтатус отправки
        /// </summary>
        public virtual EmailSendStatus SendingStatus { get; set; }

        /// <summary>
        /// Лог операции
        /// </summary>
        public virtual FileInfo LogFile { get; set; }
    }
}