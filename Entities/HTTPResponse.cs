using System.Collections.Generic;
using System.Net;
using System.Xml.Linq;

namespace SMEV3Library.Entity
{
    public class HTTPResponse
    {

        /// <summary>
        /// Содержимое ответа
        /// </summary>
        internal XElement SoapXML;


        /// <summary>
        /// Ошибка, если возникла
        /// </summary>
        internal string Error;

        /// <summary>
        /// HTTP код
        /// </summary>
        internal HttpStatusCode StatusCode;

        /// <summary>
        /// Приаттаченные файлы
        /// </summary>
        internal List<FileAttachment> Attachments;

        /// <summary>
        /// Снапшот содержимого отправленного пакета
        /// Сохраняется, если параметр storeLog = true, иначе null
        /// </summary>
        internal byte[] sendedData;

        /// <summary>
        /// Снапшот содержимого принятого пакета
        /// Сохраняется, если параметр storeLog = true, иначе null
        /// </summary>
        internal byte[] receivedData;
    }
}
