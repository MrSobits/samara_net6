using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Utils;
using Bars.GkhGji.Regions.Habarovsk.Entities;
using Bars.GkhGji.Regions.Habarovsk.Enums;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Services;
using System;
using System.IO;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;

namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    public class SMEVSNILSService : ISMEVSNILSService
    {
        #region Constants

        static XNamespace tns = @"http://kvs.pfr.com/snils-by-additionalData/1.0.1";
        static XNamespace smev = @"urn://x-artefacts-smev-gov-ru/supplementary/commons/1.0.1";
        static XNamespace pfr = @"http://common.kvs.pfr.com/1.0.0";

        #endregion

        #region Properties

        public IDomainService<SMEVSNILS> SMEVSNILSDomain { get; set; }

        public IDomainService<SMEVSNILSFile> SMEVSNILSFileDomain { get; set; }


        #endregion

        #region Fields

        private ISMEV3Service _SMEV3Service;

        private IFileManager _fileManager;


        #endregion

        #region Constructors

        public SMEVSNILSService(IFileManager fileManager, ISMEV3Service SMEV3Service)
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
        public bool SendInformationRequest(SMEVSNILS requestData, IProgressIndicator indicator = null)
        {
            try
            {
                //Очищаем список файлов
                indicator?.Report(null, 0, "Очистка старых файлов");
                SMEVSNILSFileDomain.GetAll().Where(x => x.SMEVSNILS == requestData).ToList().ForEach(x => SMEVSNILSFileDomain.Delete(x.Id));

                //формируем отправляемую xml
                indicator?.Report(null, 10, "Формирование запроса");
                XElement request = GetInformationRequestXML(requestData);
                ChangeState(requestData, RequestState.Formed);

                //
                indicator?.Report(null, 20, "Отправка запроса");
                var requestResult = _SMEV3Service.SendRequestAsync(request, null, true).GetAwaiter().GetResult();
                requestData.MessageId = requestResult.MessageId;
                SMEVSNILSDomain.Update(requestData);

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
                    SMEVSNILSDomain.Update(requestData);
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
        public bool TryProcessResponse(SMEVSNILS requestData, GetResponseResponse response, IProgressIndicator indicator = null)
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

                    var FNSresponse = response.MessagePrimaryContent.Element(tns + "SnilsByAdditionalDataResponse");
                    if (FNSresponse == null)
                    {
                        SetErrorState(requestData, "Данные отсутствуют");
                        return false;
                    }

                    if (FNSresponse.Element(tns + "Snils")?.Value == "")
                    {
                        SetErrorState(requestData, "Запрашиваемые сведения не найдены");
                        return false;
                    }


                    XElement data = FNSresponse.Element(tns + "Snils");
                    requestData.SNILS = data.Value;
                    requestData.RequestState = RequestState.ResponseReceived;
                    requestData.Answer = "Успешно";
                    SMEVSNILSDomain.Update(requestData);
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

        private XElement GetInformationRequestXML(SMEVSNILS requestData)
        {
            if (!string.IsNullOrEmpty(requestData.Series))
            {
                var result = new XElement(tns + "SnilsByAdditionalDataRequest",
               new XElement(smev + "FamilyName", requestData.Surname),
               new XElement(smev + "FirstName", requestData.Name),
               new XElement(smev + "Patronymic", requestData.PatronymicName),
               new XElement(tns + "BirthDate", requestData.BirthDate.ToString("yyyy-MM-dd")),
               new XElement(tns + "Gender", requestData.SMEVGender == SMEVGender.Male ? "Male" : "Female"),
                new XElement(smev + "PassportRF",
                 new XElement(smev + "Series", requestData.Series),
                 new XElement(smev + "Number", requestData.Number),
                 new XElement(smev + "IssueDate", requestData.IssueDate.Value.ToString("yyyy-MM-dd")),
                 new XElement(smev + "Issuer", requestData.Issuer)));

                return result;
            }
            else
            {
                var result = new XElement(tns + "SnilsByAdditionalDataRequest",
                    new XElement(smev + "FamilyName", requestData.Surname),
                    new XElement(smev + "FirstName", requestData.Name),
                    new XElement(smev + "Patronymic", requestData.PatronymicName),
                    new XElement(tns + "BirthDate", requestData.BirthDate.ToString("yyyy-MM-dd")),
                    new XElement(tns + "Gender", requestData.SMEVGender == SMEVGender.Male ? "Male" : "Female"),
                    new XElement(tns + "BirthPlace",
                      new XElement(pfr + "PlaceType", requestData.SnilsPlaceType == SnilsPlaceType.Special ? "Особое" : "Стандартное"),
                      new XElement(pfr + "Settlement", requestData.Settlement),
                      new XElement(pfr + "District", requestData.District),
                      new XElement(pfr + "Region", requestData.Region),
                      new XElement(pfr + "Country", requestData.Country)));

                return result;
            }
        }

        private void ChangeState(SMEVSNILS requestData, RequestState state)
        {
            requestData.RequestState = state;
            SMEVSNILSDomain.Update(requestData);
        }

        private void SetErrorState(SMEVSNILS requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            SMEVSNILSDomain.Update(requestData);
        }

        private void SaveFile(SMEVSNILS request, Stream data, string fileName)
        {
            //сохраняем ошибку
            SMEVSNILSFileDomain.Save(new SMEVSNILSFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVSNILS = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(data, fileName)
            });
        }

        private void SaveFile(SMEVSNILS request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет
            SMEVSNILSFileDomain.Save(new SMEVSNILSFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVSNILS = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private void SaveFile(SMEVSNILS request, XElement data, string fileName)
        {
            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            SMEVSNILSFileDomain.Save(new SMEVSNILSFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVSNILS = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            });
        }

        private void SaveException(SMEVSNILS request, Exception exception)
        {
            if (exception == null)
                return;

            SMEVSNILSFileDomain.Save(new SMEVSNILSFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVSNILS = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile("Exception.txt", ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            });
        }
        #endregion
    }
}
