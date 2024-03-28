using System.Collections.Generic;
using System.Net.Mail;

namespace Bars.Gkh.Helpers
{
    public interface IEMailSender
    {
        /// <summary>
        /// Отправка письма с настройками из основной части конфига
        /// </summary>
        /// <param name="to">Адрес получателя</param>
        /// <param name="theme">Тема</param>
        /// <param name="body">Текст</param>
        /// <param name="attachments">Вложенные файлы</param>
        void Send(string to, string theme, string body, IEnumerable<Attachment> attachments = null);
    }
}
