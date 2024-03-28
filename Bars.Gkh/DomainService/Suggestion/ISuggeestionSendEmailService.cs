using Bars.B4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.Gkh.DomainService
{
    interface ISuggestionSendEmailService
    {
        /// <summary>
        /// Отправить ответ на обращение по e-mail
        /// </summary>
        /// <param name="baseParams">EmailTo, Subject, AnswerText, Attachments</param>
        /// <returns></returns>
        BaseDataResult SendAnswerEmail(BaseParams baseParams);
    }
}
