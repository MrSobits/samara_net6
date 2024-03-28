namespace Bars.GisIntegration.Base.Tasks.SendData.HouseManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.HouseManagementAsync;
    using Bars.GisIntegration.Base.Tasks.SendData;
    using Entities.HouseManagement;

    /// <summary>
    /// Базовая задача отправки данных по домам
    /// <typeparam name="TRequestType">Тип запросов к сервису</typeparam>
    /// </summary>
    public abstract class BaseExportHouseDataTask<TRequestType> : BaseSendDataTask<TRequestType, getStateResult, HouseManagementPortsTypeAsyncClient, RequestHeader>
    {
        /// <summary>
        /// Получить результат экспорта пакета данных
        /// </summary>
        /// <param name="header">Заголовок запроса</param>
        /// <param name="ackMessageGuid">Идентификатор сообщения</param>
        /// <param name="result">Результат экспорта</param>
        /// <returns>Статус обработки запроса</returns>
        protected override sbyte GetStateResult(RequestHeader header, string ackMessageGuid, out getStateResult result)
        {
            var request = new getStateRequest { MessageGUID = ackMessageGuid };
            var soapClient = this.ServiceProvider.GetSoapClient();

            if (soapClient == null)
            {
                throw new Exception("Не удалось получить SOAP клиент");
            }

            soapClient.getState(header, request, out result);

            if (result == null)
            {
                throw new Exception("Пустой результат выполенния запроса");
            }

            return result.RequestState;
        }

        /// <summary>
        /// Обработать результат экспорта пакета данных
        /// </summary>
        /// <param name="responce">Ответ от сервиса</param>
        /// <param name="transportGuidDictByType">Словаь транспортных идентификаторов в разрезе типов</param>
        /// <returns>Результат обработки пакета</returns>
        protected override PackageProcessingResult ProcessResult(getStateResult responce, Dictionary<Type, Dictionary<string, long>> transportGuidDictByType)
        {
            var result = new PackageProcessingResult
            {
                State = PackageState.SuccessProcessed,
                Objects = new List<ObjectProcessingResult>()
            };

            foreach (var item in responce.Items)
            {
                var errorMessageItem = item as ErrorMessageType;
                var importResult = item as ImportResult;

                if (errorMessageItem != null)
                {
                    result.Objects.Add(new ObjectProcessingResult
                    {
                        State = ObjectProcessingState.Error,
                        Message = errorMessageItem.Description
                    });
                }
                else if (importResult != null)
                {
                    foreach (var responseItem in importResult.Items)
                    {
                        var importResultErrorMessageItem = responseItem as ErrorMessageType;
                        var importResultCommonResultItem = responseItem as ImportResultCommonResult;

                        if (importResultErrorMessageItem != null)
                        {
                            result.Objects.Add(new ObjectProcessingResult
                            {
                                State = ObjectProcessingState.Error,
                                Message = importResultErrorMessageItem.Description
                            });
                        }
                        else if (importResultCommonResultItem != null)
                        {
                            var processingResult = this.CheckResponseItem(importResultCommonResultItem, transportGuidDictByType);
                            result.Objects.Add(processingResult);
                        }
                        else
                        {
                            result.Objects.Add(new ObjectProcessingResult
                            {
                                State = ObjectProcessingState.Error,
                                Message = "Не удалось разобрать getStateResult"
                            });
                        }
                    }
                }
                else
                {
                    result.Objects.Add(new ObjectProcessingResult
                    {
                        State = ObjectProcessingState.Error,
                        Message = "Не удалось разобрать getStateResult"
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Обработать результат запроса
        /// </summary>
        /// <param name="responseItem">Элемент response</param>
        /// <param name="transportGuidDictByType">Словарь транспортных идентификаторов</param>
        /// <returns>Результат обработки</returns>
        private ObjectProcessingResult CheckResponseItem(ImportResultCommonResult responseItem, Dictionary<Type, Dictionary<string, long>> transportGuidDictByType)
        {
            var transportGuid = responseItem.TransportGUID;

            if (transportGuidDictByType.ContainsKey(typeof(RisHouse)) &&
                transportGuidDictByType[typeof(RisHouse)].ContainsKey(transportGuid))
            {
                var houseId = transportGuidDictByType[typeof(RisHouse)][transportGuid];
                return this.CheckResponseItem<RisHouse>(responseItem, houseId);
            }

            if (transportGuidDictByType.ContainsKey(typeof(ResidentialPremises)) &&
                transportGuidDictByType[typeof(ResidentialPremises)].ContainsKey(transportGuid))
            {
                var residentialPremisesId = transportGuidDictByType[typeof(ResidentialPremises)][transportGuid];
                return this.CheckResponseItem<ResidentialPremises>(responseItem, residentialPremisesId);
            }

            if (transportGuidDictByType.ContainsKey(typeof(NonResidentialPremises)) &&
                transportGuidDictByType[typeof(NonResidentialPremises)].ContainsKey(transportGuid))
            {
                var nonResidentialPremisesId = transportGuidDictByType[typeof(NonResidentialPremises)][transportGuid];
                return this.CheckResponseItem<NonResidentialPremises>(responseItem, nonResidentialPremisesId);
            }

            if (transportGuidDictByType.ContainsKey(typeof(RisEntrance)) &&
                transportGuidDictByType[typeof(RisEntrance)].ContainsKey(transportGuid))
            {
                var entranceId = transportGuidDictByType[typeof(RisEntrance)][transportGuid];
                return this.CheckResponseItem<RisEntrance>(responseItem, entranceId);
            }

            if (transportGuidDictByType.ContainsKey(typeof(LivingRoom)) &&
                transportGuidDictByType[typeof(LivingRoom)].ContainsKey(transportGuid))
            {
                var livingRoomId = transportGuidDictByType[typeof(LivingRoom)][transportGuid];
                return this.CheckResponseItem<LivingRoom>(responseItem, livingRoomId);
            }

            if (transportGuidDictByType.ContainsKey(typeof(RisLift)) &&
                transportGuidDictByType[typeof(RisLift)].ContainsKey(transportGuid))
            {
                var liftId = transportGuidDictByType[typeof(RisLift)][transportGuid];
                return this.CheckResponseItem<RisLift>(responseItem, liftId);
            }

            if (responseItem.GUID.IsEmpty())
            {
                var error = responseItem.Items.FirstOrDefault() as CommonResultTypeError;
                var errorNotation = error != null ? error.Description : "Вернулся пустой GUID";

                return new ObjectProcessingResult
                {
                    Description = $"Не найден объект с TransportGuid = {responseItem.TransportGUID}",
                    State = ObjectProcessingState.Error,
                    Message = errorNotation
                };
            }

            return new ObjectProcessingResult
            {
                GisId = responseItem.GUID,
                Description = $"Не найден объект с TransportGuid = {responseItem.TransportGUID}",
                State = ObjectProcessingState.Success
            };
        }

        /// <summary>
        /// Обработать результат запроса
        /// </summary>
        /// <typeparam name="T">Тип проверяемого объекта</typeparam>
        /// <param name="responseItem">Элемент response</param>
        /// <param name="entityId">Идентификатор проверяемого объекта</param>
        /// <returns>Результат обработки</returns>
        private ObjectProcessingResult CheckResponseItem<T>(CommonResultType responseItem, long entityId) where T : BaseRisEntity
        {
            var domain = this.Container.ResolveDomain<T>();

            try
            {
                if (responseItem.GUID.IsEmpty())
                {
                    var error = responseItem.Items.FirstOrDefault() as CommonResultTypeError;
                    var errorNotation = error != null ? error.Description : "Вернулся пустой GUID";

                    return new ObjectProcessingResult
                    {
                        Description = $"Объект типа {typeof(T).Name}",
                        RisId = entityId,
                        State = ObjectProcessingState.Error,
                        Message = errorNotation
                    };
                }

                var entity = domain.Get(entityId);
                entity.Guid = responseItem.GUID;

                return new ObjectProcessingResult
                {
                    Description = $"Объект типа {typeof(T).Name}",
                    GisId = responseItem.GUID,
                    RisId = entityId,
                    State = ObjectProcessingState.Success,
                    ObjectsToSave = new List<PersistentObject> { entity }
                };
            }
            finally
            {
                this.Container.Release(domain);
            }
        }

        /// <summary>
        /// Получить заголовок запроса
        /// </summary>
        /// <param name="messageGuid">Идентификатор запроса</param>
        /// <param name="package"></param>
        /// <returns>Заголовок запроса</returns>
        protected override RequestHeader GetHeader(string messageGuid, RisPackage package)
        {
            return new RequestHeader
            {
                Date = DateTime.Now,
                MessageGUID = messageGuid,
                Item = package.RisContragent.OrgPpaGuid,
                ItemElementName = ItemChoiceType.orgPPAGUID,
                IsOperatorSignature = package.IsDelegacy,
                IsOperatorSignatureSpecified = package.IsDelegacy
            };
        }
    }
}
