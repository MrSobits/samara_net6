using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Utils;
using Bars.GkhGji.Regions.Habarovsk.Entities;
using Bars.GkhGji.Regions.Habarovsk.Entities.SMEVRedevelopment;
using Bars.GkhGji.Regions.Habarovsk.Enums;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Services;
using System;
using System.IO;
using Bars.GkhGji.Regions.Habarovsk.SGIO.SMEVRedevelopment;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;
using GetResponseResponse = SMEV3Library.Entities.GetResponseResponse.GetResponseResponse;
using System.Xml.Serialization;
using System.Text;

namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    public class SMEVRedevelopmentService : ISMEVRedevelopmentService
    {
        #region Constants

        static XNamespace sgio = @"urn://ru.sgio.residentreconstructinfo/1.0.0";

        #endregion

        #region Properties

        public IDomainService<SMEVRedevelopment> SMEVRedevelopmentDomain { get; set; }

        public IDomainService<SMEVRedevelopmentFile> SMEVRedevelopmentFileDomain { get; set; }


        #endregion

        #region Fields

        private ISMEV3Service _SMEV3Service;

        private IFileManager _fileManager;


        #endregion

        #region Constructors

        public SMEVRedevelopmentService(IFileManager fileManager, ISMEV3Service SMEV3Service)
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
        public bool SendInformationRequest(SMEVRedevelopment requestData, IProgressIndicator indicator = null)
        {
            try
            {
                //Очищаем список файлов
                indicator?.Report(null, 0, "Очистка старых файлов");
                SMEVRedevelopmentFileDomain.GetAll().Where(x => x.SMEVRedevelopment == requestData).ToList().ForEach(x => SMEVRedevelopmentFileDomain.Delete(x.Id));

                //формируем отправляемую xml
                indicator?.Report(null, 10, "Формирование запроса");
                XElement request = GetInformationRequestXML(requestData);
                ChangeState(requestData, RequestState.Formed);

                //
                indicator?.Report(null, 20, "Отправка запроса");
                var requestResult = _SMEV3Service.SendRequestAsyncSGIO(request, null, true).GetAwaiter().GetResult();
                requestData.MessageId = requestResult.MessageId;
                SMEVRedevelopmentDomain.Update(requestData);

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
                    SMEVRedevelopmentDomain.Update(requestData);
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
        public bool TryProcessResponse(SMEVRedevelopment requestData, GetResponseResponse response, IProgressIndicator indicator = null)
        {
            try
            {
                //
                indicator?.Report(null, 40, "Сохранение данных для отладки");
                SaveFile(requestData, response.SendedData, "GetResponseRequest.dat");
                SaveFile(requestData, response.ReceivedData, "GetResponseResponse.dat");
                //сохраняем все файлы, которые прислал сервер
                try
                {
                    if (response.Attachments != null)
                    {
                        response.Attachments.ForEach(x => SaveFile(requestData, x.FileData, x.FileName));
                    }
                }
                catch(Exception e)
                {
                    
                }

                indicator?.Report(null, 70, "Обработка результата");
                if (response.Error != null)
                {
                    SetErrorState(requestData, $"Ошибка при отправке: {response.Error}");
                    SaveException(requestData, response.Error.Exception);
                    return false;
                }

                //ACK - ставим вдумчиво
              //  _SMEV3Service.GetAckAsyncSGIO(response.MessageId, true).GetAwaiter().GetResult();

                if (response.FaultXML != null)
                {
                    SaveFile(requestData, response.FaultXML, "Fault.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения в СМЭВ3, подробности в файле Fault.xml");
                }
                //сервер вернул ошибку?
                else if (response.AsyncProcessingStatus != null)
                {
                    SaveFile(requestData, response.AsyncProcessingStatus, "AsyncProcessingStatus.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения в СМЭВ3, подробности в приаттаченом файле AsyncProcessingStatus.xml");
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

                    var SGIOresponse = response.MessagePrimaryContent.Element(sgio + "response");
                    if (SGIOresponse == null)
                    {
                        SetErrorState(requestData, "Данные отсутствуют");
                        return false;
                    }
                    try
                    {
                        requestData.GovermentName = SGIOresponse.Element(sgio + "governmentname").Value;
                        requestData.ActDate = Convert.ToDateTime(SGIOresponse.Element(sgio + "actdate").Value);
                        requestData.ActNum = SGIOresponse.Element(sgio + "actnum").Value;
                        requestData.ObjectName = SGIOresponse.Element(sgio + "objectname").Value;
                    }
                    catch (Exception e)
                    {
                        
                    }
                    requestData.RequestState = RequestState.ResponseReceived;
                    requestData.Answer = "Успешно";
                    SMEVRedevelopmentDomain.Update(requestData);
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

        private DateTime? NullableDateParse(string value)
        {
            if (value == null)
                return null;

            DateTime result;

            return (DateTime.TryParse(value, out result) ? result : (DateTime?)null);
        }

        #endregion

        #region Private methods

        private XElement GetInformationRequestXML(SMEVRedevelopment requestData)
        {
            RequestType requestType = new RequestType
            {
                governmentname = requestData.GovermentName,
                actdate = requestData.ActDate.HasValue ? requestData.ActDate.Value.ToString("dd.MM.yyyy") : DateTime.Now.ToString("dd.MM.yyyy"),
                actnum = requestData.ActNum,
                objectname = requestData.ObjectName,
                address = requestData.RealityObject != null ? requestData.RealityObject.Address : "-",
                cadastralnumber = !string.IsNullOrEmpty(requestData.Cadastral) ? requestData.Cadastral : "-",
                ReceiverOKTMO = requestData.Municipality.Oktmo
            };
            var result = GetRequestElement(requestType);

            //var result = new XElement(sgio + "request",
            //                new XElement(sgio + "governmentname", requestData.GovermentName),
            //                new XElement(sgio + "actdate", requestData.ActDate),
            //                new XElement(sgio + "actnum", requestData.ActNum),
            //                new XElement(sgio + "objectname", requestData.ObjectName),
            //                new XElement(sgio + "address", requestData.RealityObject.Address),
            //                new XElement(sgio + "cadastralnumber", requestData.Cadastral),
            //                new XElement(sgio + "ReceiverOKTMO", requestData.Municipality.Oktmo)
            //                );

            //result.SetAttributeValue(XNamespace.Xmlns + "sgio", sgio);
            return result;
        }
        private XElement GetRequestElement(RequestType request)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(RequestType));
                    xmlSerializer.Serialize(streamWriter, request);
                    return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }
        }


        private void ChangeState(SMEVRedevelopment requestData, RequestState state)
        {
            requestData.RequestState = state;
            SMEVRedevelopmentDomain.Update(requestData);
        }

        private void SetErrorState(SMEVRedevelopment requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            SMEVRedevelopmentDomain.Update(requestData);
        }

        private void SaveFile(SMEVRedevelopment request, Stream data, string fileName)
        {
            //сохраняем ошибку
            SMEVRedevelopmentFileDomain.Save(new SMEVRedevelopmentFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVRedevelopment = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(data, fileName)
            });
        }

        private void SaveFile(SMEVRedevelopment request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет
            SMEVRedevelopmentFileDomain.Save(new SMEVRedevelopmentFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVRedevelopment = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private void SaveFile(SMEVRedevelopment request, XElement data, string fileName)
        {
            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            SMEVRedevelopmentFileDomain.Save(new SMEVRedevelopmentFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVRedevelopment = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            });
        }

        private void SaveException(SMEVRedevelopment request, Exception exception)
        {
            if (exception == null)
                return;

            SMEVRedevelopmentFileDomain.Save(new SMEVRedevelopmentFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVRedevelopment = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile("Exception.txt", ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            });
        }
        #endregion
    }
}
