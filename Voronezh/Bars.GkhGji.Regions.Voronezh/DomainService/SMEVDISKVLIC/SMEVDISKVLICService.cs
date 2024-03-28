using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using Bars.Gkh.Utils;
using Bars.GkhGji.Regions.Voronezh.Entities;
using Bars.GkhGji.Regions.Voronezh.Enums;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Services;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    public class SMEVDISKVLICService : ISMEVDISKVLICService
    {
        #region Constants

        static XNamespace ns1Namespace = @"urn://x-artefacts-fns-diskvlic/root/310-40/4.0.0";

        #endregion

        #region Properties

        public IDomainService<SMEVDISKVLIC> SMEVDISKVLICDomain { get; set; }

        public IDomainService<SMEVDISKVLICFile> SMEVDISKVLICFileDomain { get; set; }


        #endregion

        #region Fields

        private ISMEV3Service _SMEV3Service;

        private IFileManager _fileManager;


        #endregion

        #region Constructors

        public SMEVDISKVLICService(IFileManager fileManager, ISMEV3Service SMEV3Service)
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
        public bool SendInformationRequest(SMEVDISKVLIC requestData, IProgressIndicator indicator = null)
        {
            try
            {
                //Очищаем список файлов
                indicator?.Report(null, 0, "Очистка старых файлов");
                SMEVDISKVLICFileDomain.GetAll().Where(x => x.SMEVDISKVLIC == requestData).ToList().ForEach(x => SMEVDISKVLICFileDomain.Delete(x.Id));

                //формируем отправляемую xml
                indicator?.Report(null, 10, "Формирование запроса");
                XElement request = GetInformationRequestXML(requestData);
                ChangeState(requestData, RequestState.Formed);

                //
                indicator?.Report(null, 20, "Отправка запроса");
                var requestResult = _SMEV3Service.SendRequestAsync(request, null, true).GetAwaiter().GetResult();
                requestData.MessageId = requestResult.MessageId;
                SMEVDISKVLICDomain.Update(requestData);

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
                    SMEVDISKVLICDomain.Update(requestData);
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
        public bool TryProcessResponse(SMEVDISKVLIC requestData, GetResponseResponse response, IProgressIndicator indicator = null)
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

                    var FNSresponse = response.MessagePrimaryContent.Element(ns1Namespace + "DISKVLICResponse");
                    if (FNSresponse == null)
                    {
                        SetErrorState(requestData, "Данные отсутствуют");
                        return false;
                    }

                    if (FNSresponse.Element(ns1Namespace + "КодОбр")?.Value == "01")
                    {
                        SetErrorState(requestData, "Запрашиваемые сведения не найдены");
                        return false;
                    }

                    var SvFLElement = FNSresponse.Element(ns1Namespace + "СвДисквФЛ");
                    if (SvFLElement == null)
                    {
                        SetErrorState(requestData, "Секция СвДисквФЛ отсутствует");
                        return false;
                    }

                    ProcessResponseXML(requestData, SvFLElement);

                    requestData.RequestState = RequestState.ResponseReceived;
                    requestData.Answer = "Успешно";
                    SMEVDISKVLICDomain.Update(requestData);
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

        private void ProcessResponseXML(SMEVDISKVLIC requestData, XElement data)
        {
            requestData.FormDate = NullableDateParse(data.Attribute("ДатаСвед")?.Value);

            var svFL = data.Element(ns1Namespace + "СвДискв");
            requestData.EndDisqDate = NullableDateParse(svFL.Attribute("КонДискв")?.Value);
            requestData.RegNumber = svFL.Attribute("НомЗапРДЛ")?.Value;
            requestData.DisqDays = svFL.Attribute("СрокДисквДн")?.Value;
            requestData.DisqYears = svFL.Attribute("СрокДисквЛет")?.Value;
            requestData.DisqMonths = svFL.Attribute("СрокДисквМес")?.Value;
            requestData.Article = svFL.Attribute("СтКоАП")?.Value;

            var lawInfo = data.Element(ns1Namespace + "СвДискв").Element(ns1Namespace + "СудДискв");
            requestData.LawDate = NullableDateParse(lawInfo.Attribute("ДатаСуд")?.Value);
            requestData.LawName = lawInfo.Attribute("НаимСуд")?.Value;
            requestData.CaseNumber = lawInfo.Attribute("НомДело")?.Value;
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

        private XElement GetInformationRequestXML(SMEVDISKVLIC requestData)
        {
            var result = new XElement(ns1Namespace + "DISKVLICRequest",
                new XAttribute("ИдЗапрос", requestData.RequestId),
                new XElement(ns1Namespace + "СвФЛЗапр",
                    new XAttribute("МестоРожд", requestData.BirthPlace),
                    new XAttribute("ДатаРожд", requestData.BirthDate.ToString("yyyy-MM-dd")),
                        new XElement(ns1Namespace + "ФИО",
                        new XAttribute("FirstName", requestData.FirstName),
                        new XAttribute("Patronymic", requestData.Patronymic),
                        new XAttribute("FamilyName", requestData.FamilyName)
                        )
                    )
            );

            result.SetAttributeValue(XNamespace.Xmlns + "tns", ns1Namespace);
            return result;
        }

        private void ChangeState(SMEVDISKVLIC requestData, RequestState state)
        {
            requestData.RequestState = state;
            SMEVDISKVLICDomain.Update(requestData);
        }

        private void SetErrorState(SMEVDISKVLIC requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            SMEVDISKVLICDomain.Update(requestData);
        }

        private void SaveFile(SMEVDISKVLIC request, Stream data, string fileName)
        {
            //сохраняем ошибку
            SMEVDISKVLICFileDomain.Save(new SMEVDISKVLICFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVDISKVLIC = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(data, fileName)
            });
        }

        private void SaveFile(SMEVDISKVLIC request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет
            SMEVDISKVLICFileDomain.Save(new SMEVDISKVLICFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVDISKVLIC = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private void SaveFile(SMEVDISKVLIC request, XElement data, string fileName)
        {
            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            SMEVDISKVLICFileDomain.Save(new SMEVDISKVLICFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVDISKVLIC = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            });
        }

        private void SaveException(SMEVDISKVLIC request, Exception exception)
        {
            if (exception == null)
                return;

            SMEVDISKVLICFileDomain.Save(new SMEVDISKVLICFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVDISKVLIC = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile("Exception.txt", ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            });
        }
        #endregion
    }
}
