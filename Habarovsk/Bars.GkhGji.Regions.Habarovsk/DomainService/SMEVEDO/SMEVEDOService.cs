using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Utils;
using Bars.GkhGji.Regions.Habarovsk.Entities;
using Bars.GkhGji.Regions.Habarovsk.Enums;

using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Services;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;

namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    using System.Xml;

    using Bars.B4.Utils;

    using Fasterflect;

    public class SMEVEDOService : ISMEVEDOService
    {

        #region Properties
        public IDomainService<SMEVEDO> SMEVEDODomain { get; set; }

        public IDomainService<SMEVEDOFile> SMEVEDOFileDomain { get; set; }

        #endregion

        #region Fields
        private ISMEV3Service _SMEV3Service;
        private IFileManager _fileManager;
        #endregion

        #region Constructors
        public SMEVEDOService(IFileManager fileManager, ISMEV3Service SMEV3Service)
        {
            _fileManager = fileManager;
            _SMEV3Service = SMEV3Service;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Отправка запроса выписки ЕГРИП
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public bool SendInformationRequest(SMEVEDO requestData, IProgressIndicator indicator = null)
        {
            try
            {
                //Очищаем список файлов
                indicator?.Report(null, 0, "Очистка старых файлов");
                SMEVEDOFileDomain.GetAll().Where(x => x.SMEVEDO == requestData).ToList().ForEach(x => SMEVEDOFileDomain.Delete(x.Id));

                //формируем отправляемую xml
                indicator?.Report(null, 10, "Формирование запроса");
                XElement request = XElement.Parse(requestData.TextReq);
                ChangeState(requestData, RequestState.Formed);

                //
                indicator?.Report(null, 20, "Отправка запроса");
                var requestResult = _SMEV3Service.SendRequestAsync(request, null, true).GetAwaiter().GetResult();
                requestData.MessageId = requestResult.MessageId;
                SMEVEDODomain.Update(requestData);

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
                    //изменяем статус
                    //TODO: Domain.Update не работает из колбека авайта. Дать пендаль казани
                    requestData.RequestState = RequestState.Queued;
                    requestData.Answer = "Поставлено в очередь";
                    SMEVEDODomain.Update(requestData);
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
        public bool TryProcessResponse(SMEVEDO requestData, GetResponseResponse response, IProgressIndicator indicator = null)
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
                    requestData.Answer = response.MessagePrimaryContent.Value;
                    requestData.RequestState = RequestState.ResponseReceived;
                    SMEVEDODomain.Update(requestData);                   
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

        private void ChangeState(SMEVEDO requestData, RequestState state)
        {
            requestData.RequestState = state;
            SMEVEDODomain.Update(requestData);
        }

        private void SetErrorState(SMEVEDO requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            SMEVEDODomain.Update(requestData);
        }

        private void SaveFile(SMEVEDO request, Stream data, string fileName)
        {
            //сохраняем ошибку
            SMEVEDOFileDomain.Save(new SMEVEDOFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVEDO = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(data, fileName)
            });
        }

        private void SaveFile(SMEVEDO request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет
            SMEVEDOFileDomain.Save(new SMEVEDOFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVEDO = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private void SaveFile(SMEVEDO request, XElement data, string fileName)
        {
            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            SMEVEDOFileDomain.Save(new SMEVEDOFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVEDO = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            });
        }

        private void SaveException(SMEVEDO request, Exception exception)
        {
            if (exception == null)
                return;

            SMEVEDOFileDomain.Save(new SMEVEDOFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVEDO = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile("Exception.txt",
                    ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            });
        }
      
        #endregion
    }
}