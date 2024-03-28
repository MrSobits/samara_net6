using SMEV3Library.Entities;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Entities.SendRequestResponse;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SMEV3Library.Services
{
    public interface ISMEV3Service
    {
        /// <summary>
        /// Отправляет запрос в СМЭВ
        /// </summary>
        /// <param name="request">XML, вставляемая в MessagePrimaryContent</param>
        /// <param name="attachments">Прикладываемые файлы</param>
        /// <returns>SendRequestRequest, распарсеный в класс</returns>
        Task<SendRequestResponse> SendRequestAsync(XElement request, List<FileAttachment> attachments = null, bool saveLog = false);

        /// <summary>
        /// Отправляет запрос в СМЭВ
        /// </summary>
        /// <param name="request">XML, вставляемая в MessagePrimaryContent</param>
        /// <param name="attachments">Прикладываемые файлы</param>
        /// <returns>SendRequestRequest, распарсеный в класс</returns>
        Task SendResponceAsync(XElement responce, string messageId, string to, bool saveLog = false);

        /// <summary>
        /// Отправляет запрос в СМЭВ
        /// </summary>
        /// <param name="request">XML, вставляемая в MessagePrimaryContent</param>
        /// <param name="attachments">Прикладываемые файлы</param>
        /// <returns>SendRequestRequest, распарсеный в класс</returns>
        Task<SendRequestResponse> SendRequestAsyncPersonalSig(XElement request, List<FileAttachment> attachments = null, bool saveLog = false);

        /// <summary>
        /// Отправляет запрос в СМЭВ
        /// </summary>
        /// <param name="request">XML, вставляемая в MessagePrimaryContent</param>
        /// <param name="attachments">Прикладываемые файлы</param>
        /// <returns>SendRequestRequest, распарсеный в класс</returns>
        Task<SendRequestResponse> SendRequestAsyncSGIO(XElement request, List<FileAttachment> attachments = null, bool saveLog = false);

        /// <summary>
        /// Получает результат запроса. Если любой из параметров null, то без фильтра
        /// </summary>
        /// <param name="namespaceUri">target namespace схемы</param>
        /// <param name="rootElementLocalName">имя (local name) элемента</param>
        /// <returns>GetResponseResponse, распарсеный в класс</returns>
        Task<GetResponseResponse> GetResponseAsync(string namespaceUri = null, string rootElementLocalName = null, bool saveLog = false);

        /// <summary>
        /// Получает результат запроса. Если любой из параметров null, то без фильтра
        /// </summary>
        /// <param name="namespaceUri">target namespace схемы</param>
        /// <param name="rootElementLocalName">имя (local name) элемента</param>
        /// <returns>GetResponseResponse, распарсеный в класс</returns>
        Task<GetRequestRequest> GetRequestAsyncSGIO(string smevAction, string namespaceUri = null, string rootElementLocalName = null, bool saveLog = false);

        /// <summary>
        /// Получает результат запроса. Если любой из параметров null, то без фильтра
        /// </summary>
        /// <param name="namespaceUri">target namespace схемы</param>
        /// <param name="rootElementLocalName">имя (local name) элемента</param>
        /// <returns>GetResponseResponse, распарсеный в класс</returns>
        Task<GetResponseResponse> GetResponseAsyncSGIO(string smevAction, string namespaceUri = null, string rootElementLocalName = null, bool saveLog = false);

        /// <summary>
        /// Подтверждает получение запроса
        /// </summary>
        /// <param name="messageId">UUID сообщения</param>
        /// <param name="accepted">принято?</param>
        Task GetAckAsync(string messageId, bool accepted, bool saveLog = false);

        /// <summary>
        /// Подтверждает получение запроса
        /// </summary>
        /// <param name="messageId">UUID сообщения</param>
        /// <param name="accepted">принято?</param>
        Task GetAckAsyncSGIO(string messageId, bool accepted, bool saveLog = false);


        /// <summary>
        /// Подписывает файл ГОСТ 2012-256, откреплённая подпись
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <returns>Подпись</returns>
        byte[] SignFileDetached2012256(string filePath);
    }
}
