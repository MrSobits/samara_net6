using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Utils;
using Bars.GkhGji.Regions.Voronezh.Entities;
using Bars.GkhGji.Regions.Voronezh.Enums;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Services;
using System;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    public class GASUService : IGASUService
    {
        #region Constants
        static XNamespace gasu_publish = @"urn://gasu-gov-ru/publish/root/1.2.0";
        static XNamespace gasu_commons = @"urn://gasu-gov-ru/commons/1.2.0";

        #endregion

        #region Properties

        public IDomainService<GASU> GASUDomain { get; set; }

        public IDomainService<GASUData> GASUDataDomain { get; set; }

        public IDomainService<GASUFile> GASUFileDomain { get; set; }


        #endregion

        #region Fields

        private ISMEV3Service _SMEV3Service;

        private IFileManager _fileManager;


        #endregion

        #region Constructors

        public GASUService(IFileManager fileManager, ISMEV3Service SMEV3Service)
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
        public bool SendInformationRequest(GASU requestData, IProgressIndicator indicator = null)
        {
            try
            {
                //Очищаем список файлов
                indicator?.Report(null, 0, "Очистка старых файлов");
                GASUFileDomain.GetAll().Where(x => x.GASU == requestData).ToList().ForEach(x => GASUFileDomain.Delete(x.Id));

                //формируем отправляемую xml
                indicator?.Report(null, 10, "Формирование запроса");
                XElement request = GetInformationRequestXML(requestData);
                ChangeState(requestData, RequestState.Formed);

                //
                indicator?.Report(null, 20, "Отправка запроса");
                var requestResult = _SMEV3Service.SendRequestAsync(request, null, true).GetAwaiter().GetResult();
                requestData.MessageId = requestResult.MessageId;
                GASUDomain.Update(requestData);

                //
                indicator?.Report(null, 80, "Сохранение данных для отладки");
                SaveFile(requestData, requestResult.SendedData, "SendRequestRequest.dat");
                SaveFile(requestData, requestResult.ReceivedData, "SendRequestResponse.dat");

                //
                indicator?.Report(null, 90, "Обработка результата");
                if (requestResult.Error != null)
                {
                    SetErrorState(requestData, $"Ошибка при отправке: {requestResult.Error}");
                    SaveException(requestData, requestResult.Error.Exception);
                }
                else if (requestResult.FaultXML != null)
                {
                    SaveFile(requestData, requestResult.FaultXML, "Fault.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения в СМЭВ3, подробности в файле Fault.xml");
                }
                else if (requestResult.Status != "requestIsQueued")
                {
                    SetErrorState(requestData, "Ошибка при отправке: cервер вернул статус " + requestResult.Status);
                }
                else
                {
                    requestData.RequestState = RequestState.Queued;
                    requestData.Answer = "Поставлено в очередь";
                    GASUDomain.Update(requestData);
                    return true;
                }
            }
            catch (HttpRequestException)
            {
                //ошибки связи прокидываем в контроллер
                throw;
            }
            catch (Exception e)
            {
                SaveException(requestData, e);
                SetErrorState(requestData, "SendRequest exception: " + e.Message);
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
        public bool TryProcessResponse(GASU requestData, GetResponseResponse response, IProgressIndicator indicator = null)
        {
            try
            {
                //
                indicator?.Report(null, 40, "Сохранение данных для отладки");
                SaveFile(requestData, response.SendedData, "GetResponseRequest.dat");
                SaveFile(requestData, response.ReceivedData, "GetResponseResponse.dat");
                //сохраняем все файлы, которые прислал сервер
                response.Attachments.ForEach(x => SaveFile(requestData, x.FileData, x.FileName));

                indicator?.Report(null, 70, "Обработка результата");
                if (response.Error != null)
                {
                    SetErrorState(requestData, $"Ошибка при отправке: {response.Error}");
                    SaveException(requestData, response.Error.Exception);
                    return false;
                }

                //ACK - ставим вдумчиво
                _SMEV3Service.GetAckAsync(response.MessageId, true).GetAwaiter().GetResult();

                if (response.FaultXML != null)
                {
                    SaveFile(requestData, response.FaultXML, "Fault.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения в СМЭВ, подробности в файле Fault.xml");
                }
                //сервер вернул ошибку?
                else if (response.AsyncProcessingStatus != null)
                {
                    SaveFile(requestData, response.AsyncProcessingStatus, "AsyncProcessingStatus.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения в СМЭВ, подробности в приаттаченом файле AsyncProcessingStatus.xml");
                }
                //сервер отклонил запрос?
                else if (response.RequestRejected != null)
                {
                    requestData.RequestState = RequestState.ResponseReceived;
                    requestData.Answer = $"Сведения успешно размещены ИД = {requestData.Id * 15}";
                    GASUDomain.Update(requestData);
                    return true;
                    //   SaveFile(requestData, response.RequestRejected, "RequestRejected.xml");
                    //    SetErrorState(requestData, "Сервер отклонил запрос, подробности в приаттаченом файле RequestRejected.xml");
                }
                //ответ пустой?
                else if (response.MessagePrimaryContent == null)
                {
                    SetErrorState(requestData, "Сервер прислал ответ, в котором нет ни результата, ни ошибки обработки");
                }
                else
                {
                    requestData.RequestState = RequestState.ResponseReceived;
                    requestData.Answer = $"Сведения успешно размещены ИД = {requestData.Id*15}";
                    GASUDomain.Update(requestData);
                    return true;
                    //разбираем xml, которую прислал сервер
                    indicator?.Report(null, 80, "Разбор содержимого");

                    var FNSresponse = response.MessagePrimaryContent.Element(gasu_publish + "PublishResponse");
                    if (FNSresponse == null)
                    {
                        SetErrorState(requestData, "Данные отсутствуют");
                        return false;
                    }

                    if (FNSresponse.Element("RequestID")?.Value == "")
                    {
                        SetErrorState(requestData, "Запрос не обработан");
                        return false;
                    }

                    var requestIdElement = FNSresponse.Element("RequestID");
                    var edocIDelement = FNSresponse.Element("EdocID");

                    requestData.RequestState = RequestState.ResponseReceived;
                    requestData.Answer = $"Сведения успешно размещены ИД = {edocIDelement.Value}";
                    GASUDomain.Update(requestData);
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

        private XElement GetInformationRequestXML(GASU requestData)
        {
            var result = new XElement(gasu_publish + "PublishRequest",
                new XElement(gasu_commons + "AppHeader",
                  new XElement(gasu_commons + "ID", requestData.Id),
                  new XElement(gasu_commons + "DataSourceRef", "8765")),
                new XElement(gasu_publish + "MessageType", GetMessageType(requestData.GasuMessageType)),
               GetIndicators(requestData));

            result.SetAttributeValue(XNamespace.Xmlns + "gasu_publish", gasu_publish);
            result.SetAttributeValue(XNamespace.Xmlns + "gasu_commons", gasu_commons);

            return result;
        }

        private XElement[] GetIndicators(GASU request)
        {
            List<XElement> elements = new List<XElement>();
            var gasuData = GASUDataDomain.GetAll()
                .Where(x => x.GASU.Id == request.Id).ToList();
            foreach (GASUData data in gasuData)
            {
                elements.Add(new XElement(gasu_publish + "Indicator",
                    new XAttribute("name", data.Indexname), new XAttribute("providerRef", "8765"), new XAttribute("uid", data.IndexUid),
                    new XElement(gasu_commons+ "TimeSeriesLength",
                    new XElement(gasu_commons + "From", "2017-01-01"),
                    new XElement(gasu_commons + "To", DateTime.Now.ToString("yyyy-MM-dd"))),
                    new XElement(gasu_commons + "Unit",
                    new XAttribute("itemId", data.UnitMeasure.OkeiCode), new XAttribute("name", data.UnitMeasure.Name)),
                    new XElement(gasu_commons + "Calendar",
                    new XElement(gasu_commons + "Periodicity",
                    new XAttribute("nextRelease", data.GASU.DateTo.AddDays(1).ToString("yyyy-MM-dd")), new XAttribute("released", data.GASU.DateFrom.ToString("yyyy-MM-dd")), new XAttribute("value", "Halfyear")))));
            }

            return elements.ToArray();
        }

        private string GetMessageType(GasuMessageType mt)
        {
            switch (mt)
            {
                case GasuMessageType.ImportFull:
                    return "ImportFull";
                case GasuMessageType.ImportDelta:
                    return "ImportDelta";
                case GasuMessageType.Delete:
                    return "Delete";
                default:
                    return "ImportFull";
            }
        }

        private void ChangeState(GASU requestData, RequestState state)
        {
            requestData.RequestState = state;
            GASUDomain.Update(requestData);
        }

        private void SetErrorState(GASU requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            GASUDomain.Update(requestData);
        }

        private void SaveFile(GASU request, Stream data, string fileName)
        {
            //сохраняем ошибку
            GASUFileDomain.Save(new GASUFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                GASU = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(data, fileName)
            });
        }

        private void SaveFile(GASU request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет
            GASUFileDomain.Save(new GASUFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                GASU = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private void SaveFile(GASU request, XElement data, string fileName)
        {
            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            GASUFileDomain.Save(new GASUFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                GASU = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            });
        }

        private void SaveException(GASU request, Exception exception)
        {
            if (exception == null)
                return;

            GASUFileDomain.Save(new GASUFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                GASU = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile("Exception.txt", ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            });
        }
        #endregion
    }
}
