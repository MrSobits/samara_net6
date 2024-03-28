using SMEV3Library.Entities;
using SMEV3Library.Exceptions;
using SMEV3Library.SoapHttpClient.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    //TODO: сделать сервисом и зарегистрировать как синглтон в виндсоре, чтобы не создавать подключение каждый раз при отправке
    public class SMEVPropertyTypeSoapClient : IDisposable
    {
        #region  Fields

        private readonly HttpClient _httpClient = GetDefaultHttpClient();

        #endregion

        #region  Public methods

        public void Dispose() => _httpClient.Dispose();

        /// <summary>
        /// Отправляет soap запрос
        /// </summary>
        /// <param name="endpoint">endpoint</param>
        /// <param name="version">SOAP версия</param>
        /// <param name="bodies">Тело SOAP запроса</param>
        /// <param name="headers">Заголовки SOAP запроса</param>
        /// <param name="storeLog">Сохранить полностью пакеты запроса и ответа в sendedData и receivedData?</param>
        /// <param name="action">ActionHeader</param>
        internal async Task<HTTPResponse> PostAsync(
            Uri endpoint,
            SoapVersion soapVersion,
            XmlDocument doc,
            bool storeLog = false,
            string action = null)
        {
            var httpResponse = new HTTPResponse();

            try
            {
                if (endpoint == null)
                {
                    httpResponse.Error = $"endpoint не определен";
                    return httpResponse;
                }

                if (doc == null)
                {
                    httpResponse.Error = $"Тело запроса не определено";
                    return httpResponse;
                }

                string data = doc.InnerXml;
                var content = new MultipartFormDataContent();
                content.Headers.ContentType.MediaType = "multipart/related";
                content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("type", "\"application/xop+xml\""));
                content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("start", "\"<rootpart@soapui.org>\""));
                content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("start-info", "\"text/xml\""));
                action = "http://v2_4_3.incomingRequests.webservices.kernel.sx.fms.ru/incomingRequestsService_v2_4_3/bookRequestRequest";
                content.Headers.Add("ActionHeader", action);
                _httpClient.DefaultRequestHeaders.Add("SOAPAction", action);

                //Add SOAP Body
                var responceDoc = new XmlDocument();
                var body = new StringContent(data, Encoding.UTF8, "text/xml");
                body.Headers.ContentType.MediaType = "application/xop+xml";
                body.Headers.Add("Content-Transfer-Encoding", "8bit");
                body.Headers.Add("Content-ID", "<rootpart@soapui.org>");
                content.Add(body);

               
                    httpResponse.sendedData = await content.ReadAsByteArrayAsync().ConfigureAwait(false);

                // Execute call
                var responseMessage = await _httpClient.PostAsync(endpoint, content).ConfigureAwait(false);

                //сохраняем ответ
             
                    httpResponse.receivedData = await responseMessage.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

                // Parsing result
                if (responseMessage.StatusCode != HttpStatusCode.OK && responseMessage.StatusCode != HttpStatusCode.InternalServerError)
                {
                    httpResponse.Error = $"Сервер вернул http код: {responseMessage.StatusCode}";
                    return httpResponse;
                }

                string mediaType = responseMessage.Content.Headers.ContentType.MediaType;
             
                //-----пришел просто текстовый пакет-----
                if (mediaType == "text/xml")
                {
                    var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    
                    responceDoc.LoadXml(responseString);
                    httpResponse.Document = responceDoc;
                    httpResponse.StatusCode = responseMessage.StatusCode;
                    httpResponse.SoapXML = XElement.Parse(responseString);
                }
                else
                {
                    httpResponse.Error = $"Сервер прислал ответ с MediaType {mediaType}, который не поддерживается";
                }
            }
            catch (HttpRequestException e)
            {
                httpResponse.Error = e.ToString();
            }
            catch (Exception e)
            {
                httpResponse.Error = e.ToString();
            }

            return httpResponse;
        }

        #endregion

        #region Private Methods

        private XElement GetBody(XElement SoapXml)
        {
            if (SoapXml.Name.LocalName != "Envelope")
                throw new SMEV3LibraryException($"Server return XML {SoapXml.Name.LocalName}, not Envelope");

            var body = SoapXml.Element("http://schemas.xmlsoap.org/soap/envelope/" + "Body");

            if (body == null)
                throw new SMEV3LibraryException("Server return XML without Body or Fault tag");

            return body;
        }

        private async Task<FileAttachment> GetAttachment(HttpContent httpContent)
        {
            var content = await httpContent.ReadAsStreamAsync().ConfigureAwait(false);

            var ContentID = httpContent.Headers.First(x => x.Key == "Content-ID").Value.FirstOrDefault();
            if (ContentID == null)
                ContentID = "NotDetected";

            var attachment = new FileAttachment
            {
                FileName = ContentID,
                FileGuid = ContentID.Trim('<', '>').Split('@')[0].ToLower(),
                FileData = new byte[content.Length]
            };

            await content.ReadAsync(attachment.FileData, 0, (int)content.Length).ConfigureAwait(false);

            return attachment;
        }



        private static HttpClient GetDefaultHttpClient()
            => new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            },
            disposeHandler: false);

        #endregion
    }
}