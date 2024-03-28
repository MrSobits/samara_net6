using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Utils;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Services;
using System;
using System.IO;
using System.Linq;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using System.Net.Http;
using System.Xml.Linq;
using Bars.GkhGji.Regions.Voronezh.Entities.SMEVPremises;
using Bars.GkhGji.Regions.Voronezh.Enums;
using System.Xml;
using System.Xml.Serialization;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    public class SMEVPremisesService : ISMEVPremisesService
    {
        #region Constants

        static XNamespace rup = @"urn://x-artefacts-mstr-regional-unfitPremises-ru/root/1.0.0";
        static XNamespace com = @"urn://x-artefacts-mstr-regional-unfitPremises-ru/commons/1.0.0";

        #endregion

        #region Properties

        public IDomainService<SMEVPremises> SMEVPremisesDomain { get; set; }

        public IDomainService<SMEVPremisesFile> SMEVPremisesFileDomain { get; set; }

        #endregion

        #region Fields

        private ISMEV3Service _SMEV3Service;

        private IFileManager _fileManager;


        #endregion

        #region Constructors

        public SMEVPremisesService(IFileManager fileManager, ISMEV3Service SMEV3Service)
        {
            _fileManager = fileManager;
            _SMEV3Service = SMEV3Service;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Отправка запроса выписки ЕГРЮЛ
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public bool SendRequest(SMEVPremises requestData, IProgressIndicator indicator = null)
        {
            try
            {
                indicator?.Report(null, 0, "Очистка старых файлов");
                SMEVPremisesFileDomain.GetAll().Where(x => x.SMEVPremises == requestData).ToList().ForEach(x => SMEVPremisesFileDomain.Delete(x.Id));

                indicator?.Report(null, 10, "Формирование запроса");
                XElement request = GetInformationRequestXML(requestData);
                ChangeState(requestData, RequestState.Formed);

                indicator?.Report(null, 20, "Отправка запроса");
                var requestResult = _SMEV3Service.SendRequestAsync(request, null, true).GetAwaiter().GetResult();
                requestData.MessageId = requestResult.MessageId;
                SMEVPremisesDomain.Update(requestData);

                indicator?.Report(null, 80, "Сохранение данных для отладки");
                SaveFile(requestData, requestResult.SendedData, "SendRequestRequest.dat");
                SaveFile(requestData, requestResult.ReceivedData, "SendRequestResponse.dat");

                indicator?.Report(null, 90, "Обработка результата");
                if (requestResult.Error != null)
                {
                    SetErrorState(requestData, $"Ошибка при отправке: {requestResult.Error}");
                    SaveException(requestData, requestResult.Error.Exception);
                }
                else if (requestResult.FaultXML != null)
                {
                    SaveFile(requestData, requestResult.FaultXML, "Fault.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения, подробности в файле Fault.xml");
                }
                else if (requestResult.Status != "requestIsQueued")
                {
                    SetErrorState(requestData, "Ошибка при отправке: cервер вернул статус " + requestResult.Status);
                }
                else
                {
                    requestData.RequestState = RequestState.Queued;
                    requestData.Answer = "Поставлено в очередь";
                    SMEVPremisesDomain.Update(requestData);
                    return true;
                }
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception e)
            {
                SaveException(requestData, e);
                SetErrorState(requestData, "SendInformationRequest exception: " + e.Message);
            }

            return false;
        }

        /// <summary>
        /// Обработка ответа
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="response"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public bool TryProcessResponse(SMEVPremises requestData, GetResponseResponse response, IProgressIndicator indicator = null)
        {
            try
            {

                indicator?.Report(null, 40, "Сохранение данных для отладки");
                SaveFile(requestData, response.SendedData, "GetResponseRequest.dat");
                SaveFile(requestData, response.ReceivedData, "GetResponseResponse.dat");

                if (response.Attachments != null)
                {
                    response.Attachments.ForEach(x => SaveFile(requestData, x.FileData, x.FileName));
                }

                indicator?.Report(null, 70, "Обработка результата");
                if (response.Error != null)
                {
                    SetErrorState(requestData, $"Ошибка при отправке: {response.Error}");
                    SaveException(requestData, response.Error.Exception);
                    return false;
                }

                _SMEV3Service.GetAckAsync(response.MessageId, true).GetAwaiter().GetResult();

                if (response.FaultXML != null)
                {
                    SaveFile(requestData, response.FaultXML, "Fault.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения, подробности в файле Fault.xml");
                }
                //сервер вернул ошибку?
                else if (response.AsyncProcessingStatus != null)
                {
                    SaveFile(requestData, response.AsyncProcessingStatus, "AsyncProcessingStatus.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения, подробности в приаттаченом файле AsyncProcessingStatus.xml");
                }
                //сервер отклонил запрос?
                else if (response.RequestRejected != null)
                {
                    SaveFile(requestData, response.RequestRejected, "RequestRejected.xml");
                    SetErrorState(requestData, "Сервер отклонил запрос, подробности в приаттаченом файле RequestRejected.xml");
                }
                //ответ пустой?
                else if (response.MessagePrimaryContent == null)
                {
                    SetErrorState(requestData, "Сервер прислал ответ, в котором нет ни результата, ни ошибки обработки");
                }
                else
                {
                    //разбираем xml, которую прислал сервер
                    indicator?.Report(null, 80, "Разбор содержимого");

                    var PremisesResponse = response.MessagePrimaryContent.Element(rup + "MstrUnfitPremisesResponse");
                    if (PremisesResponse == null)
                    {
                        SetErrorState(requestData, "Данные отсутствуют");
                        return false;
                    }
                 
                    XDocument doc = new XDocument(PremisesResponse);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        XmlWriterSettings xws = new XmlWriterSettings();
                        xws.OmitXmlDeclaration = true;
                        xws.Indent = true;

                        using (XmlWriter xw = XmlWriter.Create(ms, xws))
                        {
                            doc.WriteTo(xw);
                        }

                        requestData.AnswerFile = _fileManager.SaveFile(ms, "Response.xml");
                    }

                    requestData.RequestState = RequestState.ResponseReceived;
                    requestData.Answer = "Успешно";
                    SMEVPremisesDomain.Update(requestData);
                    return true;
                }
            }
            catch (Exception e)
            {
                SaveException(requestData, e);
                SetErrorState(requestData, "SendInformationRequest exception: " + e.Message);
            }

            return false;
        }
        #endregion

        #region Private methods
        private XElement GetInformationRequestXML(SMEVPremises requestData)
        {

            var result = new XElement(rup + "MstrUnfitPremisesRequest",
                            new XElement(com + "OKTMO", requestData.OKTMO),
                            new XElement(com + "ActNumber", requestData.ActNumber),
                            new XElement(com + "ActDate", requestData.ActDate),
                            new XElement(com + "ActName", requestData.ActName),
                            new XElement(com + "ActDepartment", requestData.ActDepartment)
                            );

            result.SetAttributeValue(XNamespace.Xmlns + "rup", rup);
            result.SetAttributeValue(XNamespace.Xmlns + "com", com);
            return result;
        }

        private void ChangeState(SMEVPremises requestData, RequestState state)
        {
            requestData.RequestState = state;
            SMEVPremisesDomain.Update(requestData);
        }

        private void SetErrorState(SMEVPremises requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            SMEVPremisesDomain.Update(requestData);
        }

        private void SaveFile(SMEVPremises request, Stream data, string fileName)
        {
            //сохраняем ошибку
            SMEVPremisesFileDomain.Save(new SMEVPremisesFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVPremises = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(data, fileName)
            });
        }

        private void SaveFile(SMEVPremises request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет
            SMEVPremisesFileDomain.Save(new SMEVPremisesFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVPremises = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private void SaveFile(SMEVPremises request, XElement data, string fileName)
        {
            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            SMEVPremisesFileDomain.Save(new SMEVPremisesFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVPremises = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            });
        }

        private void SaveException(SMEVPremises request, Exception exception)
        {
            if (exception == null)
                return;

            SMEVPremisesFileDomain.Save(new SMEVPremisesFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVPremises = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile("Exception.txt", ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            });
        }
        #endregion
    }
}
