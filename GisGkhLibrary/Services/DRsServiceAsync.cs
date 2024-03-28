using GisGkhLibrary.Crypto;
using GisGkhLibrary.DebtRequestAsync;
using System.Net.Http;
using System;

namespace GisGkhLibrary.Services
{
    using System.Collections.Generic;

    /// <summary>
    /// Сервис управления обращениями асинхронный
    /// </summary>
    public static class DRsServiceAsync
    {


        static DebtRequestsAsyncPortClient service;

        static DebtRequestsAsyncPortClient ServiceInstance => service ?? (service = ServiceHelper<DebtRequestsAsyncPortClient, DebtRequestsAsyncPort>.MakeNew());

        /// <summary>
        /// Экспорт сведений об обращениях (создать запрос)
        /// </summary>
        public static object exportDRsReq(DateTime start , DateTime end)
        {
            List<object> periods = new List<object>();
            List<ItemsChoiceType4> ItemsElementName = new List<ItemsChoiceType4>();
            ItemsElementName.Add(ItemsChoiceType4.periodOfSendingRequest);
            periods.Add(new Period
            {
                startDate = start,
                endDate = end,
            });
            periods.Add(ResponseStatusType.NotSent);
            ItemsElementName.Add(ItemsChoiceType4.responseStatus);
            exportDSRsRequest newReq = new exportDSRsRequest
            {
                Items = periods.ToArray(),
                Id = Params.ContainerId,
                version = "14.0.0.0",
                includeResponses = false,
                includeResponsesSpecified = false,
                ItemsElementName = ItemsElementName.ToArray(),

            };         
           
            return (object)newReq;
        }

        /// <summary>
        /// Экспорт сведений об обращениях (создать запрос)
        /// </summary>
        public static object exportDRsNextPageReq(string srGuid, Period period)
        {
            
            List<object> periods = new List<object>();
            periods.Add(period);
            List<ItemsChoiceType4> ItemsElementName = new List<ItemsChoiceType4>();
            ItemsElementName.Add(ItemsChoiceType4.periodOfSendingRequest);
            periods.Add(ResponseStatusType.NotSent);
            ItemsElementName.Add(ItemsChoiceType4.responseStatus);
            ItemsElementName.Add(ItemsChoiceType4.exportSubrequestGUID);          
            periods.Add(srGuid);          

            exportDSRsRequest newReq = new exportDSRsRequest
            {
                Items = periods.ToArray(),                
                Id = Params.ContainerId,
                version = "14.0.0.0",
                includeResponses = true,
                includeResponsesSpecified = true,
                ItemsElementName = ItemsElementName.ToArray(),

            };

            return (object)newReq;
        }

        /// <summary>
        /// Экспорт сведений об обращениях (создать запрос)
        /// </summary>
        public static importDSRResponsesRequestAction exportDRsReqNoDebtAnswer(string srGuid, string executorGUID)
        {
            importDSRResponsesRequestAction act = new importDSRResponsesRequestAction
            {
                actionType = DSRResponseActionType.Send,
                subrequestGUID = srGuid,
                TransportGUID = Guid.NewGuid().ToString(),
                responseData = new ImportDSRResponseType
                {
                    hasDebt = false,
                    executorGUID = executorGUID,
                    description = "Автоматический ответ ИАС МЖФ"
                }
            };

            //importDSRResponsesRequest newReq = new importDSRResponsesRequest
            //{              
            //    Id = Params.ContainerId,
            //    version = "13.1.1.6",
            //    action = acts.ToArray(),
            //};

            return act;
        }
        /// <summary>
        /// Экспорт сведений об обращениях (создать запрос)
        /// </summary>
        public static object exportDRsReqGetAction(List<importDSRResponsesRequestAction> actionList)
        {

            importDSRResponsesRequest newReq = new importDSRResponsesRequest
            {
                Id = Params.ContainerId,
                version = "14.0.0.0",
                action = actionList.ToArray()
            };

            return (object)newReq;
        }
        /// <summary>
        /// Экспорт сведений об обращениях (создать запрос)
        /// </summary>
        public static importDSRResponsesRequestAction exportDRsReqDebtAnswer(string srGuid, string executorGUID, DebtInfoTypePerson personDebtor, List<DebtInfoTypeDocument> docs)
        {

            List<DebtInfoType> debts = new List<DebtInfoType>();
            debts.Add(new DebtInfoType
            {
                person = personDebtor,
                document = docs.ToArray()
            });
            importDSRResponsesRequestAction act = new importDSRResponsesRequestAction
            {
                actionType = DSRResponseActionType.Send,
                subrequestGUID = srGuid,
                TransportGUID = Guid.NewGuid().ToString(),
                responseData = new ImportDSRResponseType
                {
                    hasDebt = true,                    
                    Items = debts.ToArray(),
                    executorGUID = executorGUID,
                    description = "Автоматический ответ ИАС МЖФ"
                }
            };

            //importDSRResponsesRequest newReq = new importDSRResponsesRequest
            //{
            //    Id = Params.ContainerId,
            //    version = "13.1.1.6",
            //    action = acts.ToArray(),
            //};

            return act;
        }
        /// <summary>
        /// Получить статус обработки запроса
        /// </summary>
        /// <param name="MessageGUID">Идентификатор сообщения, присвоенный ГИС ЖКХ</param>
        /// <returns></returns>
        public static getStateResult GetState(string MessageGUID, string orgPPAGUID)
        {
            var request = new getStateRequest
            {
                MessageGUID = MessageGUID,
            };
            getStateResult responce;
            try
            {
                //  var tmpdata = RegOrgManager.GetOrganization("1126316006640", "745301001");

                ServiceInstance.getState(MakeHeader(orgPPAGUID), request, out responce);

                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Экспорт сведений об обращениях (отправить запрос)
        /// </summary>
        public static AckRequest exportDebtRequestSend(exportDSRsRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.exportDebtSubrequests(MakeHeader(orgPPAGUID), request, out responce);

                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Импорт сведений об обращениях (отправить запрос)
        /// </summary>
        public static AckRequest exportDebtResponceSend(importDSRResponsesRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.importResponses(MakeHeader(orgPPAGUID), request, out responce);

                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        private static RequestHeader MakeHeader(string orgPPAGUID)
        {
            RequestHeader header = new RequestHeader
            {

                Date = DateTime.Now,
                MessageGUID = Guid.NewGuid().ToString(),
                ItemElementName = ItemChoiceType.orgPPAGUID,
                Item = orgPPAGUID
            };

            return header;
        }
    }
}