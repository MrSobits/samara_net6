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

namespace SMEV3Library.SoapHttpClient
{
    //TODO: сделать сервисом и зарегистрировать как синглтон в виндсоре, чтобы не создавать подключение каждый раз при отправке
    public class SoapClient : IDisposable
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
            IEnumerable<XElement> bodies,
            IEnumerable<XElement> headers = null,
            IEnumerable<FileAttachment> attachments = null,
            bool storeLog = false,
            string action = null)
        {
            var httpResponse = new HTTPResponse();

            try
            {
                if (endpoint == null)
                {
                    httpResponse.Error = new Error($"endpoint не определен");
                    return httpResponse;
                }

                if (bodies == null || !bodies.Any())
                {
                    httpResponse.Error = new Error($"Тело запроса не определено");
                    return httpResponse;
                }

                // Get configuration based on version
                var messageConfiguration = new SoapMessageConfiguration(soapVersion);

                // Get the envelope
                var envelope = GetEnvelope(messageConfiguration);

                // Add headers
                if (headers != null && headers.Any())
                    envelope.Add(new XElement(messageConfiguration.Schema + "Header", headers));

                // Add bodies
                envelope.Add(new XElement(messageConfiguration.Schema + "Body", bodies));

                string data = envelope.ToString(SaveOptions.DisableFormatting);

                // Get HTTP content
                var content = new MultipartFormDataContent();
                content.Headers.ContentType.MediaType = "multipart/related";
                content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("type", "\"application/xop+xml\""));
                content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("start", "\"<rootpart@soapui.org>\""));
                content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("start-info", "\"text/xml\""));

                //Add SOAP Body
                var body = new StringContent(data, Encoding.UTF8, messageConfiguration.MediaType);
                body.Headers.ContentType.MediaType = "application/xop+xml";
                body.Headers.Add("Content-Transfer-Encoding", "8bit");
                body.Headers.Add("Content-ID", "<rootpart@soapui.org>");
                content.Add(body);

                // Add SOAP action if any
                if (action != null && messageConfiguration.SoapVersion == SoapVersion.Soap11)
                    content.Headers.Add("ActionHeader", action);
                else if (action != null && messageConfiguration.SoapVersion == SoapVersion.Soap12)
                    content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("ActionParameter", $"\"{action}\""));

                //Add Attachments
                //if (attachments != null)
                //    foreach (var attachment in attachments)
                //    {
                //        var fileContent = new ByteArrayContent(attachment.FileData);
                //        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                //        fileContent.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("name", $"{attachment.FileName}"));
                //        fileContent.Headers.Add("Content-Transfer-Encoding", "binary");
                //        fileContent.Headers.Add("Content-ID", $"<{attachment.FileName}>");
                //        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                //        {
                //            FileName = $"\"{attachment.FileName}\"",
                //            Name = $"\"{attachment.FileName}\""
                //        };
                //        content.Add(fileContent);
                //    }

                if (storeLog)
                    httpResponse.sendedData = await content.ReadAsByteArrayAsync().ConfigureAwait(false);

                // Execute call
                var responseMessage = await _httpClient.PostAsync(endpoint, content).ConfigureAwait(false);

                //сохраняем ответ
                if (storeLog)
                    httpResponse.receivedData = await responseMessage.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

                // Parsing result
                if (responseMessage.StatusCode != HttpStatusCode.OK && responseMessage.StatusCode != HttpStatusCode.InternalServerError)
                {
                    httpResponse.Error = new Error($"Сервер вернул http код: {responseMessage.StatusCode}");
                    return httpResponse;
                }

                string mediaType = responseMessage.Content.Headers.ContentType.MediaType;

                //-----пришел пакет с аттачами-----
                if (mediaType == "multipart/related")
                {
                    httpResponse.StatusCode = responseMessage.StatusCode;
                    //XmlDocument doc = new XmlDocument();
                    //doc.Load("D:\\or\\newfilesRequest.xml");
                    //XElement mpc = XElement.Parse(doc.InnerXml);
                    //httpResponse.SoapXML = GetBody(mpc, messageConfiguration);

                    //а если xml придет не первой?
                    
                    // TODO: Найти замену ReadAsMultipartAsync()
                    /*var responseStream = await responseMessage.Content.ReadAsMultipartAsync().ConfigureAwait(false);
                    if (responseStream.Contents.Count > 0)
                        httpResponse.SoapXML = GetBody(XElement.Load(await responseStream.Contents[0].ReadAsStreamAsync().ConfigureAwait(false)), messageConfiguration);*/

                    httpResponse.Attachments = new List<FileAttachment>();
                        /*for (int i = 1; i < responseStream.Contents.Count; i++)
                    {
                        var attachment = responseStream.Contents[i];
                        if (attachment != null)
                            httpResponse.Attachments.Add(await GetAttachment(attachment).ConfigureAwait(false));
                    }*/
                }
                //-----пришел просто текстовый пакет-----
                else if (mediaType == "text/xml")
                {
                    //пробуем получить файлы
                    try
                    {
                        // TODO: Найти замену ReadAsMultipartAsync()
                        /*var responseStream = await responseMessage.Content.ReadAsMultipartAsync().ConfigureAwait(false);
                        if (responseStream.Contents.Count > 0)
                            httpResponse.SoapXML = GetBody(XElement.Load(await responseStream.Contents[0].ReadAsStreamAsync().ConfigureAwait(false)), messageConfiguration);*/

                        httpResponse.Attachments = new List<FileAttachment>();
                        /*for (int i = 1; i < responseStream.Contents.Count; i++)
                        {
                            var attachment = responseStream.Contents[i];
                            if (attachment != null)
                                httpResponse.Attachments.Add(await GetAttachment(attachment).ConfigureAwait(false));
                        }*/
                    }
                    catch
                    {
                        
                    }


                    var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

                    httpResponse.StatusCode = responseMessage.StatusCode;
                    //XmlDocument doc = new XmlDocument();
                    //doc.Load("D:\\or\\newfilesRequest.xml");
                    //XElement mpc = XElement.Parse(doc.InnerXml);
                    //httpResponse.SoapXML = GetBody(mpc, messageConfiguration);
                    httpResponse.SoapXML = GetBody(XElement.Parse(responseString), messageConfiguration);
                }
                else
                {
                    httpResponse.Error = new Error($"Сервер прислал ответ с MediaType {mediaType}, который не поддерживается");
                }
            }
            catch (HttpRequestException)
            {
                throw; //Ошибки связи отдаем контроллеру
            }
            catch (Exception e)
            {
                httpResponse.Error = new Error(e);
            }
         
            return httpResponse;
        }

        /// <summary>
        /// Отправляет soap запрос
        /// </summary>
        /// <param name="endpoint">endpoint</param>
        /// <param name="version">SOAP версия</param>
        /// <param name="bodies">Тело SOAP запроса</param>
        /// <param name="headers">Заголовки SOAP запроса</param>
        /// <param name="storeLog">Сохранить полностью пакеты запроса и ответа в sendedData и receivedData?</param>
        /// <param name="action">ActionHeader</param>
        internal async Task<HTTPResponse> PostAsyncSGIO(string smevAction,
            Uri endpoint,
            SoapVersion soapVersion,
            IEnumerable<XElement> bodies,
            IEnumerable<XElement> headers = null,
            IEnumerable<FileAttachment> attachments = null,
            bool storeLog = false,
            string action = null)
        {
            var httpResponse = new HTTPResponse();

            try
            {
               // endpoint = new Uri("https://testsgio.govvrn.ru/smev3/v1.2");
                endpoint = new Uri("http://192.168.111.44/smev3/v1.2");
                if (endpoint == null)
                {
                    httpResponse.Error = new Error($"endpoint не определен");
                    return httpResponse;
                }

                if (bodies == null || !bodies.Any())
                {
                    httpResponse.Error = new Error($"Тело запроса не определено");
                    return httpResponse;
                }

                // Get configuration based on version
                var messageConfiguration = new SoapMessageConfiguration(soapVersion);

                // Get the envelope
                var envelope = GetEnvelope(messageConfiguration);

                // Add headers
                if (headers != null && headers.Any())
                    envelope.Add(new XElement(messageConfiguration.Schema + "Header", headers));

                // Add bodies
                envelope.Add(new XElement(messageConfiguration.Schema + "Body", bodies));

                string data = envelope.ToString(SaveOptions.DisableFormatting);

                // Get HTTP content
              
              //  content.Headers.ContentType.MediaType = "text/xml";
                //content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("type", "\"application/xop+xml\""));
                //content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("start", "\"<rootpart@soapui.org>\""));
                //content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("start-info", "\"text/xml\""));

                //Add SOAP Body //
                var content = new StringContent(data, Encoding.UTF8, "text/xml");
                //body.Headers.ContentType.MediaType = "text/xml";
                //body.Headers.Add("Content-Transfer-Encoding", "8bit");
                //body.Headers.Add("Content-ID", "<rootpart@soapui.org>");
              
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

                // Add SOAP action if any               
                action = "urn:GetResponse";
             //    content.Headers.Add("ActionHeader", action);
                _httpClient.DefaultRequestHeaders.Add("SOAPAction", smevAction);


                if (storeLog)
                    httpResponse.sendedData = await content.ReadAsByteArrayAsync().ConfigureAwait(false);

                // Execute call
                var responseMessage = await _httpClient.PostAsync(endpoint, content).ConfigureAwait(false);

                //сохраняем ответ
                if (storeLog)
                    httpResponse.receivedData = await responseMessage.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

                // Parsing result
                if (responseMessage.StatusCode != HttpStatusCode.OK && responseMessage.StatusCode != HttpStatusCode.InternalServerError)
                {
                    httpResponse.Error = new Error($"Сервер вернул http код: {responseMessage.StatusCode}");
                    return httpResponse;
                }

                string mediaType = responseMessage.Content.Headers.ContentType.MediaType;

                //-----пришел пакет с аттачами-----
                if (mediaType == "multipart/related")
                {
                    httpResponse.StatusCode = responseMessage.StatusCode;
                    
                    //а если xml придет не первой?
                    // TODO: Найти замену ReadAsMultipartAsync()
                   /* var responseStream = await responseMessage.Content.ReadAsStreamAsync().ReadAsMultipartAsync().ConfigureAwait(false);
                    if (responseStream.Contents.Count > 0)
                        httpResponse.SoapXML = GetBody(XElement.Load(await responseStream.Contents[0].ReadAsStreamAsync().ConfigureAwait(false)), messageConfiguration);

                    httpResponse.Attachments = new List<FileAttachment>();
                    for (int i = 1; i < responseStream.Contents.Count; i++)
                    {
                        var attachment = responseStream.Contents[i];
                        if (attachment != null && attachment.Headers.ContentType.MediaType == "application/octet-stream")
                            httpResponse.Attachments.Add(await GetAttachment(attachment).ConfigureAwait(false));
                    }*/
                }
                //-----пришел просто текстовый пакет-----
                else if (mediaType == "text/xml")
                {
                    var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

                    httpResponse.StatusCode = responseMessage.StatusCode;
                    //XmlDocument doc = new XmlDocument();
                    //doc.Load("D:\\or\\newfilesRequest.xml");
                    //XElement mpc = XElement.Parse(doc.InnerXml);
                    //httpResponse.SoapXML = GetBody(mpc, messageConfiguration);
                     httpResponse.SoapXML = GetBody(XElement.Parse(responseString), messageConfiguration);
                }
                else
                {
                    httpResponse.Error = new Error($"Сервер прислал ответ с MediaType {mediaType}, который не поддерживается");
                }
            }
            catch (HttpRequestException)
            {
                throw; //Ошибки связи отдаем контроллеру
            }
            catch (Exception e)
            {
                httpResponse.Error = new Error(e);
            }

            return httpResponse;
        }

        #endregion

        #region Private Methods

        private XElement GetBody(XElement SoapXml, SoapMessageConfiguration messageConfiguration)
        {
            if (SoapXml.Name.LocalName != "Envelope")
                throw new SMEV3LibraryException($"Server return XML {SoapXml.Name.LocalName}, not Envelope");

            var body = SoapXml.Element(messageConfiguration.Schema + "Body");

            if (body == null)
                throw new SMEV3LibraryException("Server return XML without Body or Fault tag");

            return body;
        }

        private async Task<FileAttachment> GetAttachment(HttpContent httpContent)
        {
            var content = await httpContent.ReadAsStreamAsync().ConfigureAwait(false);

            var ContentID = httpContent.Headers.First(x => x.Key.ToLower() == "content-id").Value.FirstOrDefault();
            if (ContentID == null)
                ContentID = "NotDetected";

            var attachment = new FileAttachment
            {
                FileName = ContentID,
           //     FileGuid = ContentID.Trim('<', '>').Split('@')[0].ToLower(),
                FileData = new byte[content.Length]
            };

            await content.ReadAsync(attachment.FileData, 0, (int)content.Length).ConfigureAwait(false);

            return attachment;
        }

        private static XElement GetEnvelope(SoapMessageConfiguration soapMessageConfiguration)
        {
            return new
                XElement(
                    soapMessageConfiguration.Schema + "Envelope",
                    new XAttribute[] {
                        new XAttribute(XNamespace.Xmlns + "soapenv", "http://schemas.xmlsoap.org/soap/envelope/")
                        ,new XAttribute(XNamespace.Xmlns + "ns", "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.2")
                        ,new XAttribute(XNamespace.Xmlns + "ns1", "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.2")
                    });
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