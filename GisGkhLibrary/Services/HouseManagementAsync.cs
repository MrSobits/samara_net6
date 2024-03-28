using GisGkhLibrary.Crypto;
using GisGkhLibrary.Exceptions;
using GisGkhLibrary.HouseManagementAsync;
using System;
using System.Linq;

namespace GisGkhLibrary.Services
{
    using Org.BouncyCastle.Asn1.Ocsp;
    using System.Collections.Generic;

    /// <summary>
    /// Сервис управления помещениями асинхронный
    /// </summary>
    public static class HouseManagementAsyncService
    {


        static HouseManagementPortsTypeAsyncClient service;

        static HouseManagementPortsTypeAsyncClient ServiceInstance => service ?? (service = ServiceHelper<HouseManagementPortsTypeAsyncClient, HouseManagementPortsTypeAsync>.MakeNew());

        /// <summary>
        /// Импорт списка лицевых счетов (создать запрос)
        /// </summary>
        public static object importAccountDataReq(importAccountRequestAccount[] accounts)
        {


            var request = new importAccountRequest
            {
                Account = accounts,
                Id = Params.ContainerId,
                version = "10.0.1.1"
            };
            return (object)request;
        }

        /// <summary>
        /// Импорт списка лицевых счетов (отправить запрос)
        /// </summary>
        public static AckRequest importAccountDataSend(importAccountRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.importAccountData(MakeHeader(orgPPAGUID), request, out responce);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Получить перечень общесистемных справочников с указанием даты последнего изменения каждого из них (создать запрос)
        /// </summary>
        public static object exportAccountDataReq(Guid HouseGuid)
        {
            var request = new exportAccountRequest
            {
                ItemsElementName = new ItemsChoiceType17[]
                {
                    ItemsChoiceType17.FIASHouseGuid
                },
                Items = new string[]
                {
                    HouseGuid.ToString()
                },
                Id = Params.ContainerId,
                version = "10.0.1.1"
            };
            return (object)request;
        }

        /// <summary>
        /// Получить перечень общесистемных справочников с указанием даты последнего изменения каждого из них (отправить запрос)
        /// </summary>
        public static AckRequest exportAccountDataSend(exportAccountRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.exportAccountData(MakeHeader(orgPPAGUID), request, out responce);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Получить перечень общесистемных справочников с указанием даты последнего изменения каждого из них (создать запрос)
        /// </summary>
        public static object exportBriefApartmentHouseReq(Guid HouseGuid)
        {
            var request = new exportBriefApartmentHouseRequest
            {
                HouseGuid = HouseGuid.ToString(),
                version = "12.2.1.2",
                Id = Params.ContainerId
            };
            return (object)request;
        }

        /// <summary>
        /// Получить перечень общесистемных справочников с указанием даты последнего изменения каждого из них (отправить запрос)
        /// </summary>
        public static AckRequest exportBriefApartmentHouseSend(exportBriefApartmentHouseRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.exportBriefApartmentHouse(MakeHeader(orgPPAGUID), request, out responce);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Экспорт данных дома (создание запроса)
        /// </summary>
        /// <param name="HouseGuid">ФИАС дома</param>
        /// <returns></returns>
        public static object exportHouseDataReq(Guid HouseGuid)
        {
            var request = new exportHouseRequest
            {
                version = "12.2.0.1",
                Id = Params.ContainerId,
                FIASHouseGuid = HouseGuid.ToString()
            };

            return request;
        }

        /// <summary>
        /// Экспорт данных дома (отправка запроса)
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="orgPPAGUID">GUID организации</param>
        /// <returns></returns>
        public static AckRequest exportHouseDataSend(exportHouseRequest request, string orgPPAGUID)
        {
            AckRequest response;

            try
            {
                ServiceInstance.exportHouseData(MakeHeader(orgPPAGUID), request, out response);
                return response;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
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

        private static RequestHeader MakeHeader(string orgPPAGUID)
        {
            //var orgPPAGUID = RegOrgManager.GetOrganization(Params.OGRN, Params.KPP).First().PPAGUID.ToString();
            //var tmpdata = RegOrgManager.GetOrganization("1107451016860", "745301001");
            //var orgPPAGUID = tmpdata.First().PPAGUID.ToString();
            //Минсвязи ЧО
            //var orgPPAGUID = "9d029b46-f205-42ca-bdee-4485274b3264";
            //ФКР ЧО
            //var orgPPAGUID = "52d55897-b16b-4bd0-9ed2-e6ce8f9c81e8";
            return new RequestHeader
            {
                
                Date = DateTime.Now,
                MessageGUID = Guid.NewGuid().ToString(),
                ItemElementName = ItemChoiceType1.orgPPAGUID,
                Item = orgPPAGUID
                //IsOperatorSignature = true,
                //IsOperatorSignatureSpecified = true
            };
        }
    }
}
